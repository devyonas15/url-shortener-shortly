export interface UrlSuccessModalProps {
  shortUrl: string | undefined;
  isOpen: boolean;
  onCopyToClipboard: (isNotificationOpen: boolean) => void;
  onClose: () => void;
}

export interface UrlGenerationFormProps {
  onShortUrlGenerated: (url: string | undefined) => void;
  onSuccessfulSubmit: (isSuccessModalOpen: boolean) => void;
}
