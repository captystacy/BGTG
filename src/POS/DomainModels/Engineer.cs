using System.ComponentModel.DataAnnotations;

namespace POS.DomainModels;

public enum Engineer
{
    [Display(Name = "", ShortName = "")]
    Unknown = 0,
    [Display(Name = "Ю.В. Черота", ShortName = "Черота")]
    Cherota = 1,
    [Display(Name = "С.С. Капитан", ShortName = "Капитан")]
    Kapitan = 2,
    [Display(Name = "С.М. Прищеп", ShortName = "Прищеп")]
    Prishep = 3,
    [Display(Name = "А.Е. Селиванова", ShortName = "Селиванова")]
    Selivanova = 4,
    [Display(Name = "С.М. Сайко", ShortName = "Сайко")]
    Saiko = 5,
    [Display(Name = "Я.В. Близнюк", ShortName = "Близнюк")]
    Bliznuk = 6,
}