using ActionImplementations;
using EventSourcing.Core;
using WebShopWebApi.Commands;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Handlers;

public class PlaceAnOrder : Command<OrderDTO> { }
