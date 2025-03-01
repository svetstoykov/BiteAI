import React from "react";

const NotFoundPage: React.FC = () => {
  return (
    <div className="p-10 text-center bg-white shadow-lg rounded-2xl">
      <h1 className="text-5xl font-bold text-red-600">404</h1>
      <p className="mt-4 text-2xl text-gray-700">Page Not Found</p>
      <a
        href="/"
        className="inline-block px-6 py-2 mt-6 text-lg duration-300 text-white transition bg-moss-green rounded-lg hover:bg-moss-green/80"
      >
        Go Home
      </a>
    </div>
  );
};

export default NotFoundPage;
