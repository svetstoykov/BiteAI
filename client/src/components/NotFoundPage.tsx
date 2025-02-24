import React from "react";

const NotFoundPage: React.FC = () => {
  return (
    <div className="fixed inset-0 flex items-center justify-center">
      <div className="p-10 text-center bg-white shadow-lg rounded-2xl">
        <h1 className="text-5xl font-bold text-red-600">404</h1>
        <p className="mt-4 text-2xl text-gray-700">Page Not Found</p>
        <a
          href="/"
          className="inline-block px-6 py-2 mt-6 text-lg text-white transition bg-pastel-green rounded-lg hover:bg-pastel-green/70"
        >
          Go Home
        </a>
      </div>
    </div>
  );
};

export default NotFoundPage;
