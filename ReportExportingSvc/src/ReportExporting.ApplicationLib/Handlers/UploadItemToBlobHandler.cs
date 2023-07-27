﻿using ReportExporting.ApplicationLib.Entities;
using ReportExporting.ApplicationLib.Services;

namespace ReportExporting.ApplicationLib.Handlers;

public class UploadItemToBlobHandler: IUploadItemToBlobHandler
{
    private readonly IBlobStorageService _blobStorageService;

    public UploadItemToBlobHandler(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<ReportRequestObject> Handle(Stream fileStream, ReportRequestObject request)
    {
        try
        {
            var response = await _blobStorageService.UploadExportFileAync(fileStream, request.FileName);

            if (response.HasValue)
            {
                request.Progress.Add(ExportingProgress.UploadFileToBlob);
            }
            else
            {
                request.Progress.Add(ExportingProgress.FailUploadingFileToBlob);
                request.Status = ExportingStatus.Failure;
            }
        }
        catch (Exception)
        {
            request.Progress.Add(ExportingProgress.FailUploadingFileToBlob);
            request.Status = ExportingStatus.Failure;
        }

        return request;
    }
}