const Footer = () => {
  return (
    <footer className="w-full px-8 pt-10 pb-5 bg-eggshell fixed bottom-0 left-0">
      <div className="flex justify-between">
        <h1 className="text-5xl">
          <span className="font-thin">Bite</span>
          <span>AI</span>
        </h1>
        <ul className="flex gap-4 text-lg">
          <li>
            <a href="#">Terms</a>
          </li>
          <li>
            <a href="#">Privacy</a>
          </li>
          <li>
            <a href="#">Support</a>
          </li>
          <li>
            <a href="#">Contact</a>
          </li>
        </ul>
      </div>
      <div className="text-center mt-2 text-dark-charcoal/80">
        © 2025 BiteAI. All rights reserved.
      </div>
    </footer>
  );
};

export default Footer;
