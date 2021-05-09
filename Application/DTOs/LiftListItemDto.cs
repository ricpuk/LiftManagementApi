using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class LiftListItemDto
    {
        public LiftListItemDto(Lift lift)
        {
            Id = lift.Id;
            Floor = lift.CurrentFloor;
            LiftState = lift.State;
        }
        public int Id { get; set; }
        public int Floor { get; set; }
        public LiftState LiftState { get; set; }
    }
}
