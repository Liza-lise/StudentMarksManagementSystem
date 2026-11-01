
namespace StudentMarksManagementSystem.Models
{
    public class Module
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ModuleType Type { get; set; } = ModuleType.Examinable;
    }
}
