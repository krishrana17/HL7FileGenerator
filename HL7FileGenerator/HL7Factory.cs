using HL7.Dotnetcore;
using Bogus;

namespace HL7FileGenerator;

public class HL7Factory
{
    private Faker faker;

    public HL7Factory() 
    {
        faker = new();
    }

    public void GenerateHL7Files(string outputPath, int numberOfFiles)
    {
        try
        {
            var fakerData = PrepareFakerModel();

            for (int i = 0; i < numberOfFiles; i++)
            {
                var patientInfo = GeneratePatientInfo(fakerData);

                var message = new HL7MessageBuilder()
                                .AddMSHSegment()
                                .AddPIDSegment(fakerData, patientInfo)
                                .AddPV1Segment(fakerData)
                                .AddIN1Segment(fakerData, patientInfo)
                                .AddGT1Segment(patientInfo)
                                .AddORCSegment(fakerData)
                                .Build();

                ProduceHL7Files(message, outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Generate fake patient information using Bogus
    /// </summary>
    /// <returns></returns>
    private (string PatientId, string DOB, string PatientName, string PatientAddress) GeneratePatientInfo(FakerDataModel fakerData)
    {
        var patientId = fakerData.FakerRandom.Replace("#######");
        var dob = fakerData.FakerDate.Between(DateTime.Today.AddYears(-90), DateTime.Today.AddYears(-18)).ToString("yyyyMMdd");
        var patientName = $"{fakerData.FakerName.LastName()}^{fakerData.FakerName.FirstName()}";
        var patientAddress = $"{fakerData.FakerAddress.BuildingNumber()} {fakerData.FakerAddress.StreetName()}^^{fakerData.FakerAddress.City()}^{fakerData.FakerAddress.StateAbbr()}^{fakerData.FakerAddress.ZipCode()}";

        return (patientId, dob, patientName, patientAddress);
    }

    /// <summary>
    /// Generate HL7 files and saved it at given output path.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="outputPath"></param>
    private void ProduceHL7Files(Message message, string outputPath)
    {
        string fileName = $"HL7_{Guid.NewGuid()}.hl7";
        string filePath = Path.Combine(outputPath, fileName);
        File.WriteAllText(filePath, SerializeHL7Message(message));
    }

    /// <summary>
    /// Return the serialized HL7 content from Message object.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private string SerializeHL7Message(Message message)
    {
        return message.SerializeMessage(false);
    }

    /// <summary>
    /// Return FakerDataModel object
    /// </summary>
    /// <returns></returns>
    private FakerDataModel PrepareFakerModel()
    {
        return new()
        {
            FakerRandom = faker.Random,
            FakerName = faker.Name,
            FakerAddress = faker.Address,
            FakerPhone = faker.Phone,
            FakerCompany = faker.Company,
            FakerDate = faker.Date
        };
    }
}
