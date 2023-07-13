using HL7.Dotnetcore;

namespace HL7FileGenerator;

public class HL7MessageBuilder
{
    private Message message;

    public HL7MessageBuilder()
    {
        message = new Message();
    }

    /// <summary>
    /// Add MSH Segment
    /// </summary>
    /// <returns></returns>
    public HL7MessageBuilder AddMSHSegment()
    {
        message.AddSegmentMSH("", "", "", "", "", "ADT^A02", DateTime.Now.ToString("yyyyMMddHHmmss"), "P", "2.3");
        return this;
    }

    /// <summary>
    /// Return the final message object containing HL7 information
    /// </summary>
    /// <returns></returns>
    public Message Build()
    {
        return message;
    }

    /// <summary>
    /// Create the PID segment
    /// </summary>
    /// <param name="faker"></param>
    /// <returns></returns>
    public HL7MessageBuilder AddPIDSegment(FakerDataModel fakerData, (string PatientId, string DOB, string PatientName, string PatientAddress) patientInfo)
    {
        var pidSegment = new Segment("PID", new HL7Encoding());
        pidSegment.AddNewField("1", 1);
        pidSegment.AddNewField($"{patientInfo.PatientId}^^^2", 3); // Patient ID
        pidSegment.AddNewField($"{patientInfo.PatientId}^^^2", 2); // Alternate Patient ID
        pidSegment.AddNewField(patientInfo.PatientName, 5); // Patient Name
        pidSegment.AddNewField(patientInfo.DOB, 7); // Patient DOB
        pidSegment.AddNewField("M", 8); // Patient Gender
        pidSegment.AddNewField(patientInfo.PatientAddress, 11); // Patient Address
        pidSegment.AddNewField(fakerData.FakerRandom.Replace("####-#"), 10); // Patient Race
        pidSegment.AddNewField(fakerData.FakerAddress.CountryCode(), 12); // Patient Country code
        pidSegment.AddNewField(fakerData.FakerPhone.PhoneNumber(), 13); // Patient Home Phone number
        pidSegment.AddNewField(fakerData.FakerRandom.Replace("###-##-####"), 19); // Patient SSN Number
        pidSegment.AddNewField("EN", 15);
        pidSegment.AddNewField("S", 16);
        pidSegment.AddNewField("N", 22);

        pidSegment.AddNewField(fakerData.FakerRandom.Replace("######"), 18);
        message.AddNewSegment(pidSegment);

        return this;
    }

    /// <summary>
    /// Add PV1 Segment
    /// </summary>
    /// <returns></returns>
    public HL7MessageBuilder AddPV1Segment(FakerDataModel fakerData)
    {
        var pv1Segment = new Segment("PV1", new HL7Encoding());
        pv1Segment.AddNewField(fakerData.FakerRandom.Replace("######"), 3); // Preadmit Number
        pv1Segment.AddNewField($"{fakerData.FakerRandom.Replace("##########")}^{fakerData.FakerName.LastName()}^{fakerData.FakerName.FirstName()}", 7); // Attending Doctor
        message.AddNewSegment(pv1Segment);
        return this;
    }

    /// <summary>
    /// Add IN1 Segment
    /// </summary>
    /// <returns></returns>
    public HL7MessageBuilder AddIN1Segment(FakerDataModel fakerData, (string PatientId, string DOB, string PatientName, string PatientAddress) patientInfo)
    {
        var in1Segment = new Segment("IN1", new HL7Encoding());
        in1Segment.AddNewField("1", 1);
        in1Segment.AddNewField(fakerData.FakerCompany.CompanyName(), 4); // Insurance Name
        in1Segment.AddNewField($"{fakerData.FakerAddress.BuildingNumber()} {fakerData.FakerAddress.StreetName()}^^{fakerData.FakerAddress.City()}^{fakerData.FakerAddress.StateAbbr()}^{fakerData.FakerAddress.ZipCode()}", 5); //  Insurance Company Address
        in1Segment.AddNewField(patientInfo.PatientName, 16); // Insured Person
        in1Segment.AddNewField("01", 17); // Insured Relation
        in1Segment.AddNewField(patientInfo.DOB, 18); // Insured Date of Birth
        in1Segment.AddNewField(patientInfo.PatientAddress, 19);
        in1Segment.AddNewField(fakerData.FakerRandom.Replace("############"), 36); // Insurance Policy Number
        message.AddNewSegment(in1Segment);
        return this;
    }

    /// <summary>
    /// Add GT1 Segment
    /// </summary>
    /// <returns></returns>
    public HL7MessageBuilder AddGT1Segment((string PatientId, string DOB, string PatientName, string PatientAddress) patientInfo)
    {
        var gt1Segment = new Segment("GT1", new HL7Encoding());
        gt1Segment.AddNewField("1", 1);
        gt1Segment.AddNewField(patientInfo.PatientName, 3);
        gt1Segment.AddNewField("01^SELF", 11);
        gt1Segment.AddNewField(patientInfo.DOB, 8);
        gt1Segment.AddNewField(patientInfo.PatientAddress, 5);
        gt1Segment.AddNewField("M", 9);
        message.AddNewSegment(gt1Segment);
        return this;
    }

    /// <summary>
    /// Add ORC Segment
    /// </summary>
    /// <returns></returns>
    public HL7MessageBuilder AddORCSegment(FakerDataModel fakerData)
    {
        var orcSegment = new Segment("ORC", new HL7Encoding());
        orcSegment.AddNewField("NW", 1); // New Order
        orcSegment.AddNewField(DateTime.Now.ToString("yyyyMMddhhmmss"), 9);
        orcSegment.AddNewField($"{fakerData.FakerRandom.Replace("##########")}^{fakerData.FakerName.LastName()}^{fakerData.FakerName.FirstName()}", 12); // Ordering Provider
        message.AddNewSegment(orcSegment);
        return this;
    }
}
