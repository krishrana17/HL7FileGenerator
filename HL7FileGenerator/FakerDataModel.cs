using Bogus.DataSets;
using Bogus;

namespace HL7FileGenerator;

public class FakerDataModel
{
    public Randomizer FakerRandom { get; set; } = null!;

    public Date FakerDate { get; set; } = null!;

    public Address FakerAddress { get; set; } = null!;
    
    public Name FakerName { get; set; } = null!;
    
    public Company FakerCompany { get; set; } = null!;
    
    public PhoneNumbers FakerPhone { get; set; } = null!;
}
