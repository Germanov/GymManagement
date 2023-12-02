using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Application.Gyms.Queries.GetGym;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[Route("subscriptions/{subscriptionId:guid}/gyms")]
public class GymsController : ApiController
{
    private readonly ISender mediator;

    public GymsController(ISender mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGym(CreateGymRequest request, Guid subscriptionId)
    {
        var command = new CreateGymCommand(request.Name, subscriptionId);

        var createGymResult = await mediator.Send(command);

        return createGymResult.Match(
            gym => CreatedAtAction(
                nameof(GetGym),
                new { subscriptionId, GymId = gym.Id },
                new GymResponse(gym.Id, gym.Name)),
            Problem);
    }

    [HttpGet("{gymId:guid}")]
    public async Task<IActionResult> GetGym(Guid subscriptionId, Guid gymId)
    {
        var command = new GetGymQuery(subscriptionId, gymId);

        var getGymResult = await mediator.Send(command);

        return getGymResult.Match(
            gym => Ok(new GymResponse(gym.Id, gym.Name)),
            Problem);
    }
}
