﻿using ReportExporting.ApplicationLib.Entities;
using ReportExporting.ApplicationLib.Services;

namespace ReportExporting.ApplicationLib.Handlers;

public class DownloadItemFromBlobHandler: IDownloadItemFromBlobHandler
{
    private readonly IBlobStorageService _blobStorageService;

    public DownloadItemFromBlobHandler(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<ReportRequestObject> Handle(Stream fileStream, ReportRequestObject request)
    {
        if (request.Status == ExportingStatus.Failure)
            return request;

        try
        {
            var response = await _blobStorageService.DownloadExportFileAync(request.FileName, fileStream);

            if (response.Status == 204)
            {
                request.Progress.Add(ExportingProgress.UploadFileToBlob);
            }
            else
            {
                request.Progress.Add(ExportingProgress.FailDownloadingBlobToStream);
                request.Status = ExportingStatus.Failure;
            }
        }
        catch (Exception)
        {
            request.Progress.Add(ExportingProgress.FailDownloadingBlobToStream);
            request.Status = ExportingStatus.Failure;
        }

        return request;
    }
}