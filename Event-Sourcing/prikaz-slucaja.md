# Prikaz slucaja
## Uvod u projekat 4.1

Our case study centers on a microservice-based shipping application designed to demonstrate how to balance availability and consistency using CQRS, Event Sourcing, the State pattern, and the Outbox pattern. The system orchestrates four primary domains—Order, Inventory, Payment, and Shipping—each deployed as an independent service. Users interact with an API Gateway which routes REST-style HTTP requests to the appropriate microservice controller. Internally, each controller emits a business command; the command handler produces one or more domain events which are persisted atomically in an event store and in an outbox table within the same database transaction.

The shipping application supports two core business functionalities:

    Order Placement

        Command: PlaceOrderCommand

        Event Sequence:

            OrderPlaced (an order record is created)

            InventoryReserved (requested items are moved from “available” to “reserved”)

            PaymentInitiated (the payment service begins processing)

    Order Completion & Shipping

        Command: CompleteOrderCommand

        Event Sequence:

            PaymentSucceeded (payment confirmation received)

            InventoryRemoved (reserved items are deducted from stock)

            OrderCompleted (order moves to “ready for shipment”)

            ShippingStarted (shipping workflow is triggered)

All generated events are emitted as a batch inside a single database transaction: either every event and the corresponding outbox entries are committed, or none are. Once committed, an outbox poller publishes these events to the message broker, where projection components update read models and downstream services react.

In the following pages, we will illustrate the full Use Case Diagram for these two functionalities, detail the class-level data model and UML component diagram, and then describe, module by module, how each service implements its part of the process—highlighting command handlers, state transitions, event storage, outbox dispatch, and projection updates.

# 4.2 Specifikacija zahteva (Requirements Specification)

In UML use-case modeling, an actor is any external entity—whether a human user or another system—that interacts with the boundary of our application by initiating a use case or by receiving its outcomes. In our shipping application, although most logic runs inside Web API controllers and microservices, we still identify the key “roles” that drive or respond to the two commands (PlaceOrderCommand and CompleteOrderCommand):

    Customer (Consumer)

        Role: The end user of the application.

        Interaction: Invokes both high-level commands via the REST API:

            PlaceOrderCommand to start the order placement flow

            CompleteOrderCommand to finalize payment and trigger shipping

        Why an actor?: Initiates the entire workflow and waits for confirmation (order ID, payment status, tracking link).

    Warehouse System

        Role: Represents the external inventory management system that holds physical stock.

        Interaction: Receives InventoryReserved and InventoryRemoved events (produced by our Inventory module) and updates its own records accordingly.

        Why an actor?: Although we implement an Inventory microservice internally, conceptually it stands in for an external stock-management system that must be notified.

    Payment Provider

        Role: A third-party payment gateway (e.g. Stripe, PayPal).

        Interaction:

            When PaymentInitiated is emitted, our Payment module calls its API.

            It then returns a callback or webhook leading to PaymentSucceeded (or failure) inside our system.

        Why an actor?: It is an external system that actually processes money and notifies us of outcomes.

    Delivery Operator

        Role: The logistics partner or shipping carrier.

        Interaction: Listens for the ShippingStarted event and arranges physical pickup and delivery; may later send status updates (e.g. “in transit”, “delivered”) back to our system.

        Why an actor?: Though we run a Shipping microservice to coordinate it, the real-world carrier is an external participant that completes the last mile.

    Note: In our diagram these roles map to modules inside the system—Inventory Module stands in for Warehouse System, Payment Module for Payment Provider, and Shipping Module for Delivery Operator—but in the use-case view they’re drawn as external actors to show clear boundaries and responsibilities.
