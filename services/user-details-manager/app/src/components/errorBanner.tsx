interface ErrorBannerProps {
  message: string;
}

export default function ErrorBanner({ message }: ErrorBannerProps) {
  if (!message) {
    return null;
  }

  return (
    <div
      className="w-full bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-md"
      role="alert"
    >
      <p>{message}</p>
    </div>
  );
}
