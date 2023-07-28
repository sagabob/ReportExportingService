﻿using ReportExporting.ApplicationLib.Entities;

namespace ReportExporting.ApplicationLib.Helpers;

public class ReportRequestTableEntityFactory
{
    public static ReportRequestTableEntity CreateTableEntity(ReportRequestObject request)
    {
        return new ReportRequestTableEntity
        {
            FileName = request.FileName,
            EmailAddress = request.EmailAddress,
            PartitionKey = request.Id.ToString(),
            RowKey = request.Id.ToString(),
            Status = Enum.GetName(typeof(ExportingStatus), request.Status),
            FullProgress = request.GetFullProgress()
        };
    }
}