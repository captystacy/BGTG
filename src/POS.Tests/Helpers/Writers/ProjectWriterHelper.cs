using System;
using System.IO;
using Moq;
using POS.Infrastructure.Writers.Base;
using POS.Models;
using POS.ViewModels;

namespace POS.Tests.Helpers.Writers
{
    public static class ProjectWriterHelper
    {
        public static Mock<IProjectWriter> GetMock()
        {
            return new Mock<IProjectWriter>();
        }

        public static Mock<IProjectWriter> GetMock(ProjectViewModel viewModel, DateTime constructionStartDate, EmployeesNeed employeesNeed, DurationByLC durationByLC)
        {
            var projectWriter = GetMock();

            projectWriter
                .Setup(x => x.GetProjectStream(viewModel, constructionStartDate, employeesNeed, durationByLC))
                .ReturnsAsync(new MemoryStream());

            return projectWriter;
        }
    }
}
