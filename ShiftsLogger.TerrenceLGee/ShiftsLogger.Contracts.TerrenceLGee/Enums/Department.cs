using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Contracts.TerrenceLGee.Enums;

public enum Department
{
    [Display(Name = "Development")]
    Development,
    [Display(Name = "Information Technology")]
    InformationTechnology,
    [Display(Name = "Marketing")]
    Marketing,
    [Display(Name = "Sales")]
    Sales
}
