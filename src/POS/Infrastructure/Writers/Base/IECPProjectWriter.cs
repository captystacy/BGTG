namespace POS.Infrastructure.Writers.Base;

public interface IECPProjectWriter
{
    MemoryStream Write(Stream durationByLCStream, Stream calendarPlanStream, Stream energyAndWaterStream, string objectCipher, string templatePath);
    int GetNumberOfEmployees(Stream durationByLCStream);
}