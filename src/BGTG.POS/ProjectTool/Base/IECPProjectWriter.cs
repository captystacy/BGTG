using System;
using BGTG.POS.DurationTools.DurationByLCTool;

namespace BGTG.POS.ProjectTool.Base
{
    public interface IECPProjectWriter
    {
        void Write(string objectCipher, DurationByLC durationByLC,
            DateTime constructionStartDate, string durationByLCPath, string calendarPlanPath, string energyAndWaterPath,
            string templatePath, string savePath);
    }
}