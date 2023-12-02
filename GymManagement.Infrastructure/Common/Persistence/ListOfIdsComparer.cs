using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GymManagement.Infrastructure.Common.Persistence;

public class ListOfIdsComparer : ValueComparer<List<Guid>>
{
    public ListOfIdsComparer()
        : base(
        (v1, v2) => v1!.SequenceEqual(v2!),
        v => v.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        v => v)
    {
    }
}
