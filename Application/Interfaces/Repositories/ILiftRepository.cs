using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ILiftRepository
    {
        /// <summary>
        /// Gets lift by specified id.
        /// </summary>
        /// <param name="id">Id of the lift.</param>
        /// <returns></returns>
        Lift GetById(int id);

        /// <summary>
        /// Tries to add a lift to repository.
        /// </summary>
        /// <param name="lift">Lift to add.</param>
        /// <returns>true if lift was added. false if lift with id already exists.</returns>
        bool Add(Lift lift);
    }
}
