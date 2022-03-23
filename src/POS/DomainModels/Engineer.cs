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
    [Display(Name = "О.Н. Пигальская", ShortName = "Пигальская")]
    Pigalskaya = 7,
    [Display(Name = "П.Н. Гомонов", ShortName = "Гомонов")]
    Gomonov = 8,
    [Display(Name = "В.М. Каленик", ShortName = "Каленик")]
    Kalenik = 9,
    [Display(Name = "А.А. Игнатенко", ShortName = "Игнатенко")]
    Ignatenko = 10,
    [Display(Name = "Д.Н. Дмитрик", ShortName = "Дмитрик")]
    Dmitrik = 11,
    [Display(Name = "А.В. Морозюк", ShortName = "Морозюк")]
    Morozuk = 12,
    [Display(Name = "О.В. Вусик", ShortName = "Вусик")]
    Vusik = 13,
}