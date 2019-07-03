using System;
using Quaver.Shared.Config;
using Quaver.Shared.Helpers;
using Steamworks;
using Wobble.Logging;

namespace Quaver.Shared.Online
{
    public class SteamWorkshopSkin : IDisposable
    {
        /// <summary>
        ///     The handle used to start making calls to upload the skin
        /// </summary>
        private UGCUpdateHandle_t Handle { get; set; }

        /// <summary>
        ///     The title of the skin
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     The description of the skin
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     The path to the file
        /// </summary>
        public string PreviewFilePath { get; }

        /// <summary>
        ///     The path to the skin folder
        /// </summary>
        public string SkinFolderPath { get; }

        /// <summary>
        ///     Any sort of notes to provide for the skin update
        /// </summary>
        public string PatchNotes { get; }

        /// <summary>
        ///     Called after calling ISteamUGC::CreateItem.
        /// </summary>
        private CallResult<CreateItemResult_t> OnCreateItemResponse { get; }

        /// <summary>
        ///     Called when submitting the workshop update has completed
        /// </summary>
        /// <returns></returns>
        private CallResult<SubmitItemUpdateResult_t> OnSubmitUpdateResponse { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="previewFilePath"></param>
        /// <param name="skin"></param>
        /// <param name="patchNotes"></param>
        public SteamWorkshopSkin(string title, string description, string previewFilePath, string skin, string patchNotes = null)
        {
            Title = title;
            Description = description;
            PreviewFilePath = previewFilePath;
            SkinFolderPath = $"{ConfigManager.SkinDirectory.Value}/{skin}/".Replace("\\", "/");
            PatchNotes = patchNotes;

            OnCreateItemResponse = CallResult<CreateItemResult_t>.Create(OnCreateItemResultCallResponse);

            var resp = SteamUGC.CreateItem((AppId_t) SteamManager.ApplicationId, EWorkshopFileType.k_EWorkshopFileTypeCommunity);
            OnCreateItemResponse.Set(resp);
        }

        /// <summary>
        ///     Called when after calling SteamUGC.CreateItem();
        /// </summary>
        /// <param name="result"></param>
        /// <param name="bIOfailure"></param>
        private void OnCreateItemResultCallResponse(CreateItemResult_t result, bool bIOfailure)
        {
            if (bIOfailure)
            {
                Logger.Error("Failed to Create workshop item:\n" +
                             $"m_eResult: {result.m_eResult}\n" +
                             $"m_nPublishedFileId: {result.m_nPublishedFileId}\n" +
                             $"m_bUserNeedsToAcceptWorkshopLegalAgreement: {result.m_bUserNeedsToAcceptWorkshopLegalAgreement}", LogType.Network);

                return;
            }

            // Open in Steam client to accept legal agreement for workshop
            if (result.m_bUserNeedsToAcceptWorkshopLegalAgreement)
                BrowserHelper.OpenURL($"steam://url/CommunityFilePage/{result.m_nPublishedFileId}");

            Handle = SteamUGC.StartItemUpdate((AppId_t) SteamManager.ApplicationId, result.m_nPublishedFileId);

            SteamUGC.SetItemTitle(Handle, Title);
            SteamUGC.SetItemDescription(Handle, Description);
            SteamUGC.SetItemVisibility(Handle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);

            if (PreviewFilePath != null)
                SteamUGC.SetItemPreview(Handle, PreviewFilePath);

            SteamUGC.SetItemContent(Handle, SkinFolderPath);

            // Start updating to Steam
            var call = SteamUGC.SubmitItemUpdate(Handle, PatchNotes);

            OnSubmitUpdateResponse = CallResult<SubmitItemUpdateResult_t>.Create(OnSubmittedItemUpdate);
            OnSubmitUpdateResponse.Set(call);
        }

        /// <summary>
        ///     Called after submitting an update
        /// </summary>
        /// <param name="result"></param>
        /// <param name="bIOfailure"></param>
        private void OnSubmittedItemUpdate(SubmitItemUpdateResult_t result, bool bIOfailure)
        {
            if (bIOfailure)
            {
                Logger.Error("Failed to Create workshop item:\n" +
                             $"m_eResult: {result.m_eResult}\n" +
                             $"m_nPublishedFileId: {result.m_nPublishedFileId}\n" +
                             $"m_bUserNeedsToAcceptWorkshopLegalAgreement: {result.m_bUserNeedsToAcceptWorkshopLegalAgreement}", LogType.Network);

                return;
            }

            Logger.Important($"Workshop upload result: {result.m_eResult}", LogType.Network);

            if (result.m_eResult == EResult.k_EResultOK)
            {
                Logger.Important($"Workshop upload successful!", LogType.Network);
            }
            else
            {
                Logger.Important($"Workshop upload failed!", LogType.Network);
            }
        }

        /// <summary>
        ///     Returns the upload progress percentage
        /// </summary>
        /// <returns></returns>
        public int GetUploadProgressPercentage()
        {
            SteamUGC.GetItemUpdateProgress(Handle, out var bytesProcessed, out var bytesTotal);
            return (int) (bytesProcessed / bytesTotal * 100);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            OnCreateItemResponse?.Dispose();
            OnSubmitUpdateResponse?.Dispose();
        }
    }
}