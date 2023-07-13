Console.Write("Enter the number of HL7 files to generate: ");
int numberOfFiles = int.Parse(Console.ReadLine()!);

Console.Write("Enter the output directory path: ");
string outputPath = Console.ReadLine()!;

var hl7Factory = new HL7Factory();
hl7Factory.GenerateHL7Files(outputPath, numberOfFiles);

Console.WriteLine("HL7 files generated successfully.");