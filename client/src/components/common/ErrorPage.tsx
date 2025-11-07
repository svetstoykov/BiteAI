import React from "react";

interface ErrorPageProps {
  title?: string;
  message?: string;
  primaryButton?: {
    text: string;
    onClick: () => void;
  };
  secondaryButton?: {
    text: string;
    onClick: () => void;
  };
}

const ErrorPage: React.FC<ErrorPageProps> = ({
  title = "Oops!",
  message = "Something went wrong",
  primaryButton,
  secondaryButton
}) => {
  return (
    <div className="p-10 text-center bg-white shadow-lg rounded-2xl">
      <h1 className="text-5xl font-bold text-red-600">{title}</h1>
      <p className="mt-4 text-2xl text-gray-700">{message}</p>
      {(primaryButton || secondaryButton) && (
        <div className="mt-6 space-x-4">
          {primaryButton && (
            <button
              onClick={primaryButton.onClick}
              className="inline-block px-6 py-2 text-lg duration-300 text-white transition bg-moss-green rounded-lg hover:bg-moss-green/80"
            >
              {primaryButton.text}
            </button>
          )}
          {secondaryButton && (
            <button
              onClick={secondaryButton.onClick}
              className="inline-block px-6 py-2 text-lg duration-300 text-gray-700 transition bg-gray-200 rounded-lg hover:bg-gray-300"
            >
              {secondaryButton.text}
            </button>
          )}
        </div>
      )}
    </div>
  );
};

export default ErrorPage;