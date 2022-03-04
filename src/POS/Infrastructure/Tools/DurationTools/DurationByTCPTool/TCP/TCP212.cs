using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP.Models;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP;

public static class TCP212
{
    public static readonly Appendix AppendixA = new("Нормы продолжительности строительства городских инженерных сетей и сооружений", 8, 'А', new List<PipelineCategory>
    {
        new("Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с откосами", new List<PipelineComponent>
        {
            new(new List<string> { "стальных труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 500, "до 500"), new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 2M, 0.3M),
                    new(1M, 2.5M, 0.3M),
                    new(1.5M, 4M, 0.5M),
                }),
                new(new DiameterRange(500, 1000, "600-900"), new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 2M, 0.3M),
                    new(1M, 3M, 0.3M),
                    new(1.5M, 4M, 0.5M),
                }),
                new(new DiameterRange(1000, 1400, "1000-1200"), new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 2M, 0.3M),
                    new(1M, 3.5M, 0.3M),
                    new(1.5M, 5M, 0.5M),
                }),
                new(new DiameterRange(1400, 1601, "1400-1600"), new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 2.5M, 0.3M),
                    new(1M, 4M, 0.3M),
                    new(1.5M, 6M, 0.5M),
                }),
            }),
            new(new List<string> { "чугунных труб", "асбестоцементных труб", "керамических труб", "бестонных труб", "железобетонных труб", "стеклопластиковых труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 500, "до 500"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 3M, 0.3M),
                    new(1M, 3.5M, 0.3M),
                    new(1.5M, 5.5M, 0.5M),
                }),
                new(new DiameterRange(500, 1000, "600-900"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.6M, 3M, 0.3M),
                    new(1M, 4M, 0.3M),
                    new(1.5M, 5.5M, 0.5M),
                }),
                new(new DiameterRange(1000, 1400, "1000-1200"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 3M, 0.3M),
                    new(1M, 5M, 0.3M),
                    new(1.5M, 7M, 0.5M),
                }),
                new(new DiameterRange(1400, 1601, "1400-1600"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 3.5M, 0.3M),
                    new(1M, 5.5M, 0.3M),
                    new(1.5M, 8.5M, 0.5M),
                }),
            }),
            new(new List<string> { "полиэтиленовых труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 300, "до 300"), new List<PipelineStandard>
                {
                    new(0.5M, 1.5M, 0.3M),
                    new(1M, 2M, 0.2M),
                    new(2M, 2.5M, 0.2M),
                    new(3M, 3.5M, 0.2M),
                }),
                new(new DiameterRange(500, 1000, "600-900"), new List<PipelineStandard>
                {
                    new(0.5M, 2M, 0.3M),
                    new(1M, 2.5M, 0.3M),
                    new(2M, 3M, 0.3M),
                    new(3M, 4M, 0.3M),
                }),
            })
        }),
        new("Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с креплением стенок", new List<PipelineComponent>
        {
            new(new List<string> { "стальных труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 500, "до 500"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 2.5M, 0.3M),
                    new(1M, 3.5M, 0.3M),
                    new(1.5M, 5.5M, 0.5M),
                }),
                new(new DiameterRange(500, 1000, "600-800"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 2.5M, 0.3M),
                    new(1M, 4.5M, 0.3M),
                    new(1.5M, 6M, 0.5M),
                }),
                new(new DiameterRange(1000, 1400, "1000-1200"), new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 3M, 0.3M),
                    new(1M, 5M, 0.3M),
                    new(1.5M, 7.5M, 0.5M),
                }),
                new(new DiameterRange(1400, 1601, "1400-1600"), new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 3.5M, 0.3M),
                    new(1M, 6M, 0.3M),
                    new(1.5M, 8.5M, 0.5M),
                }),
            }),
            new(new List<string> { "чугунных труб", "асбестоцементных труб", "керамических труб", "бестонных труб", "железобетонных труб", "стеклопластиковых труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 500, "до 500"), new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 3.5M, 0.3M),
                    new(1M, 4.5M, 0.3M),
                    new(1.5M, 6.5M, 0.5M),
                }),
                new(new DiameterRange(500, 1000, "600-900"), new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 3.5M, 0.3M),
                    new(1M, 6M, 0.3M),
                    new(1.5M, 8M, 0.5M),
                }),
                new(new DiameterRange(1000, 1400, "1000-1200"), new List<PipelineStandard>
                {
                    new(0.1M, 2.5M, 0.3M),
                    new(0.5M, 4M, 0.3M),
                    new(1M, 7M, 0.3M),
                    new(1.5M, 9.5M, 0.5M),
                }),
                new(new DiameterRange(1400, 1601, "1400-1600"), new List<PipelineStandard>
                {
                    new(0.1M, 2.5M, 0.3M),
                    new(0.5M, 5M, 0.3M),
                    new(1M, 8M, 0.3M),
                    new(1.5M, 11M, 0.5M),
                }),
            }),
            new(new List<string> { "полиэтиленовых труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 300, "до 300"), new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(1M, 2.5M, 0.2M),
                    new(2M, 3.5M, 0.2M),
                    new(3M, 5.5M, 0.2M),
                }),
                new(new DiameterRange(300, 600, "до 600"), new List<PipelineStandard>
                {
                    new(0.5M, 3M, 0.3M),
                    new(1M, 3.5M, 0.3M),
                    new(2M, 4.5M, 0.3M),
                    new(3M, 6.5M, 0.3M),
                }),
            })
        }),
        new("Уличные тепловые сети, сооружаемые в траншеях с откосами", new List<PipelineComponent>
        {
            new(new List<string> { "сборных железобетонных лотковых элементов" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 400, "до 400"),  new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 3M, 0.3M),
                    new(1M, 6M, 0.3M),
                    new(1.5M, 8.5M, 0.5M),
                }),
                new(new DiameterRange(400, 600, "400-600"),  new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 3.5M, 0.3M),
                    new(1M, 7M, 0.3M),
                    new(1.5M, 10M, 0.5M),
                }),
                new(new DiameterRange(600, 800, "600-800"),  new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 4M, 0.3M),
                    new(1M, 7M, 0.3M),
                    new(1.5M, 10M, 0.5M),
                }),
                new(new DiameterRange(800, 1000, "800-1000"),  new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 5M, 0.3M),
                    new(1M, 9M, 0.3M),
                    new(1.5M, 13M, 0.5M),
                }),
                new(new DiameterRange(1000, 1200, "1000-1200"),  new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 5.5M, 0.3M),
                    new(1M, 10M, 0.3M),
                    new(1.5M, 14.5M, 0.5M),
                }),
                new(new DiameterRange(1200, 1400, "1200-1400"),  new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 6M, 0.3M),
                    new(1M, 11.5M, 0.3M),
                    new(1.5M, 17M, 0.5M),
                }),
            })
        }),
        new("Уличные тепловые сети, сооружаемые в траншеях с креплением стенок", new List<PipelineComponent>
        {
            new(new List<string> { "сборных железобетонных лотковых элементов" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 400, "до 400"), new List<PipelineStandard>
                {
                    new(0.1M, 1M, 0.3M),
                    new(0.5M, 4M, 0.3M),
                    new(1M, 7.5M, 0.3M),
                    new(1.5M, 10.5M, 0.5M),
                }),
                new(new DiameterRange(400, 600, "400-600"),  new List<PipelineStandard>
                {
                    new(0.1M, 1.5M, 0.3M),
                    new(0.5M, 4.5M, 0.3M),
                    new(1M, 8.5M, 0.3M),
                    new(1.5M, 12M, 0.5M),
                }),
                new(new DiameterRange(600, 800, "600-800"),  new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 5M, 0.3M),
                    new(1M, 9M, 0.3M),
                    new(1.5M, 13.5M, 0.5M),
                }),
                new(new DiameterRange(800, 1000, "800-1000"),  new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 6.5M, 0.3M),
                    new(1M, 12M, 0.3M),
                    new(1.5M, 17M, 0.5M),
                }),
                new(new DiameterRange(1000, 1200, "1000-1200"),  new List<PipelineStandard>
                {
                    new(0.1M, 2M, 0.3M),
                    new(0.5M, 7M, 0.3M),
                    new(1M, 13.5M, 0.3M),
                    new(1.5M, 20M, 0.5M),
                }),
                new(new DiameterRange(1200, 1400, "1200-1400"),  new List<PipelineStandard>
                {
                    new(0.1M, 2.5M, 0.3M),
                    new(0.5M, 8M, 0.3M),
                    new(1M, 15M, 0.3M),
                    new(1.5M, 22.5M, 0.5M),
                }),
            })
        }),
    });

    public static readonly Appendix AppendixB = new("Нормы продолжительности строительства объектов коммунального хозяйства", 14, 'Б', new List<PipelineCategory>
    {
        new("Наружные трубопроводы", new List<PipelineComponent>
        {
            new(new List<string> { "стальных труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 400, "до 400"),new List<PipelineStandard>
                {
                    new(1M, 2M, 0M),
                    new(2M, 3M, 0M),
                    new(5M, 4M, 0M),
                    new(10M, 6M, 0M),
                }),
                new(new DiameterRange(400, 1200, "800"),new List<PipelineStandard>
                {
                    new(2M, 3M, 0M),
                    new(5M, 5M, 0M),
                    new(10M, 8M, 1M),
                    new(30M, 12M, 2M),
                    new(50M, 14M, 2M),
                }),
                new(new DiameterRange(1200, 1600, "1200"),new List<PipelineStandard>
                {
                    new(2M, 4M, 1M),
                    new(5M, 7M, 1M),
                    new(10M, 11M, 1M),
                    new(30M, 17M, 2M),
                    new(50M, 19M, 2M),
                }),
                new(new DiameterRange(1600, int.MaxValue, "1600"),new List<PipelineStandard>
                {
                    new(2M, 5M, 1M),
                    new(5M, 9M, 1M),
                    new(10M, 14M, 1M),
                    new(30M, 21M, 2M),
                    new(50M, 24M, 2M),
                }),
            }),
            new(new List<string> { "полиэтиленовых труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 301, "300"),new List<PipelineStandard>
                {
                    new(1M, 1.5M, 0M),
                    new(2M, 2M, 0M),
                    new(5M, 4M, 0M),
                }),
                new(new DiameterRange(301, 600, "до 600"),new List<PipelineStandard>
                {
                    new(1M, 2M, 0M),
                    new(2M, 2.5M, 0M),
                    new(5M, 4.5M, 0M),
                })
            }),
            new(new List<string> { "чугунных труб", "асбестоцементных труб", "керамических труб", "бестонных труб", "стеклопластиковых труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 800, "500"),new List<PipelineStandard>
                {
                    new(1M, 3M, 0M),
                    new(2M, 4M, 0M),
                    new(4M, 5M, 0M),
                    new(6M, 7M, 0M),
                }),
                new(new DiameterRange(800, 1000, "800"),new List<PipelineStandard>
                {
                    new(2M, 5M, 0M),
                    new(4M, 7M, 0M),
                    new(6M, 9M, 1M),
                    new(15M, 15M, 2M),
                    new(30M, 23M, 2M),
                    new(50M, 25M, 2M),
                }),
                new(new DiameterRange(1000, int.MaxValue, "1000"),new List<PipelineStandard>
                {
                    new(2M, 6.5M, 0M),
                    new(4M, 9M, 1M),
                    new(6M, 12M, 1M),
                    new(15M, 18M, 2M),
                    new(30M, 27M, 2M),
                    new(50M, 30M, 2M),
                }),
            }),
            new(new List<string> { "железобетонных труб" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 800, "500"),new List<PipelineStandard>
                {
                    new(1M, 3M, 0M),
                    new(2M, 4M, 0M),
                    new(4M, 5M, 0M),
                    new(6M, 7M, 0M),
                }),
                new(new DiameterRange(800, 1000, "800"),new List<PipelineStandard>
                {
                    new(2M, 5M, 0M),
                    new(4M, 7M, 0M),
                    new(6M, 9M, 1M),
                    new(15M, 15M, 2M),
                    new(30M, 23M, 2M),
                    new(50M, 25M, 2M),
                }),
                new(new DiameterRange(1000, 1600, "1000"),new List<PipelineStandard>
                {
                    new(2M, 6.5M, 0M),
                    new(4M, 9M, 1M),
                    new(6M, 12M, 1M),
                    new(15M, 18M, 2M),
                    new(30M, 27M, 2M),
                    new(50M, 30M, 2M),
                }),
                new(new DiameterRange(1600, 2400, "1600"),new List<PipelineStandard>
                {
                    new(2M, 7M, 0M),
                    new(4M, 10M, 1M),
                    new(6M, 14M, 1M),
                    new(15M, 24M, 2M),
                    new(30M, 35M, 2M),
                    new(50M, 39M, 2M),
                }),
                new(new DiameterRange(2400, 3500, "2400"),new List<PipelineStandard>
                {
                    new(2M, 10M, 1M),
                    new(4M, 14M, 2M),
                    new(6M, 19M, 2M),
                    new(15M, 24M, 2M),
                    new(30M, 31M, 2M),
                    new(50M, 38M, 2M),
                }),
                new(new DiameterRange(3500, int.MaxValue, "3500"),new List<PipelineStandard>
                {
                    new(2M, 11M, 2M),
                    new(4M, 16M, 2M),
                    new(6M, 22M, 2M),
                    new(15M, 27M, 2M),
                    new(30M, 36M, 2M),
                    new(50M, 44M, 2M),
                }),
            }),
        }),
        new("Распределительная газовая сеть", new List<PipelineComponent>
        {
            new(new List<string> { "стальных труб в две нитки" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 200, "до 200"),new List<PipelineStandard>
                {
                    new(1M, 2M, 0.1M),
                    new(3M, 3M, 0.2M),
                }),
                new(new DiameterRange(200, 601, "200-600"),new List<PipelineStandard>
                {
                    new(1M, 2.5M, 0.1M),
                    new(3M, 3.5M, 0.2M),
                }),
            }),
            new(new List<string> { "стальных труб в одну нитку" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 200, "до 200"),new List<PipelineStandard>
                {
                    new(1M, 1M, 0.1M),
                    new(3M, 2M, 0.2M),
                    new(10M, 5M, 0.5M),
                }),
                new(new DiameterRange(200, 601, "200-600"),new List<PipelineStandard>
                {
                    new(1M, 1.5M, 0.1M),
                    new(3M, 3M, 0.2M),
                    new(10M, 8.5M, 0.5M),
                }),
            }),
            new(new List<string> { "полиэтиленовых труб в одну нитку" }, new List<PipelineCharacteristic>
            {
                new(new DiameterRange(0, 200, "до 200"),new List<PipelineStandard>
                {
                    new(1M, 1M, 0.1M),
                    new(3M, 1.5M, 0.2M),
                    new(10M, 3.5M, 0.5M),
                }),
            }),
        }),
    });
}