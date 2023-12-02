using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GymManagement.Infrastructure.Common.Persistence;

public class ListOfIdsConverter : ValueConverter<List<Guid>, string>
{
    public ListOfIdsConverter(ConverterMappingHints? mappingHints = null)
        : base(
            values => string.Join(',', values),
            values => values.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList(),
            mappingHints)
    {
    }
}
