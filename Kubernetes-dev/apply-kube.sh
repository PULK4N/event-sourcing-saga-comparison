kubectl apply -f ./kafka-deployment.yml
kubectl apply -f ./account-manager-deployment.yml
kubectl apply -f ./audit-manager-deployment.yml
kubectl apply -f ./namespace.yml
kubectl apply -f ./notification-manager-deployment.yml
kubectl apply -f ./saga-orchestrator-deployment.yml
kubectl apply -f ./transaction-manager-deployment.yml
