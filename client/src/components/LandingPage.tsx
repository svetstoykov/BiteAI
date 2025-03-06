import { motion } from "framer-motion";
import { Apple, Calculator, Carrot, Salad, Utensils, Weight } from "lucide-react";
import React from "react";

const BiteAILanding: React.FC = () => {
  const features = [
    {
      title: "AI-Powered Meal Planning",
      description:
        "Our cutting-edge AI generates weekly curated meal plans tailored specifically to your preferences, dietary restrictions, and nutritional goals. Every meal is balanced, delicious, and designed to help you succeed.",
      icon: Utensils,
    },
    {
      title: "Precise Calorie Calculation",
      description:
        "Set your weight goals with confidence using our accurate calorie tracking system. We consider your age, activity level, and metabolic factors to determine the perfect calorie targets for your goals.",
      icon: Calculator,
    },
    {
      title: "Target Weight Management",
      description:
        "Tell us your goal weight, and we'll create a sustainable pathway to achieve it. Our system adjusts your meal plans over time as you progress, ensuring you're always on track toward your target.",
      icon: Weight,
    },
    {
      title: "Multi-Diet Support",
      description:
        "Whether you're following keto, paleo, vegetarian, vegan, or any other diet, our AI adapts to your preferences. We generate menus that respect your dietary choices while optimizing nutrition.",
      icon: Apple,
    },
  ];

  const dietOptions = [
    { name: "Keto Plans", icon: Salad },
    { name: "Vegan Options", icon: Carrot },
    { name: "Weight Loss", icon: Weight },
    { name: "Balanced Diet", icon: Utensils },
  ];

  return (
    <div>
      <section className="mb-10 mt-5">
        <div className="text-center">
          <h1 className="text-6xl font-bold">Personalized Nutrition, Powered by AI</h1>
          <p className="text-xl font-thin mt-5 max-w-[700px] mx-auto">
            BiteAI creates customized meal plans designed to help you meet your nutrition
            goals using advanced AI technology. Whether you want to lose weight, gain
            muscle, or maintain a healthy lifestyle, we've got you covered.
          </p>
          <button
            className="hover:bg-dark-charcoal/90 transition duration-300 font-thin mt-10 p-3 px-5 rounded-full bg-dark-charcoal text-white"
            aria-label="Start your nutrition journey"
          >
            Start Your Journey
          </button>
        </div>
      </section>

      <section className="pt-20">
        <h2 className="text-center text-4xl font-bold">
          Designed for Your Nutritional Success
        </h2>
        <div className="container mx-auto px-4 py-12">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            {features.map((feature, index) => {
              const Icon = feature.icon;

              return (
                <motion.div
                  key={index}
                  className="bg-white rounded-xl shadow-lg border border-gray-100 p-6"
                  whileHover={{
                    y: -8,
                    boxShadow:
                      "0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)",
                    transition: { duration: 0.2 },
                  }}
                >
                  <div className="flex items-start">
                    <div className="bg-blue-50 p-3 rounded-lg mr-4">
                      <Icon className="h-6 w-6" />
                    </div>
                    <div className="flex-1">
                      <h3 className="text-lg font-bold text-gray-900 mb-2">
                        {feature.title}
                      </h3>
                      <p className="text-gray-600 text-sm leading-relaxed">
                        {feature.description}
                      </p>
                    </div>
                  </div>
                </motion.div>
              );
            })}
          </div>
        </div>
      </section>

      <section className="py-16 px-4">
        <div className="max-w-5xl mx-auto text-center">
          <h2 className="text-4xl font-bold mb-4">Start Your Nutrition Journey</h2>
          <p className="text-gray-600 text-lg mb-8 max-w-2xl mx-auto">
            Join thousands of users who have transformed their relationship with food
            through personalized, AI-driven meal planning.
          </p>

          <button
            className="hover:bg-dark-charcoal/90 transition duration-300 font-thin mb-10 p-3 px-5 rounded-full bg-dark-charcoal text-white text-xl"
            aria-label="Create Your Plan"
          >
            Create Your Plan
          </button>

          <nav className="diet-options">
            <ul className="grid grid-cols-4 gap-4 max-w-4xl mx-auto">
              {dietOptions.map((option, index) => {
                const Icon = option.icon;

                return (
                  <motion.li
                    key={index}
                    className="flex flex-col items-center justify-center border border-gray-200 bg-white rounded-lg p-6 h-40 cursor-pointer shadow-sm"
                    whileHover={{
                      y: -8,
                      boxShadow:
                        "0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)",
                      transition: { duration: 0.2 },
                    }}
                  >
                    <div className="mb-4">
                      <Icon className="h-10 w-10 text-black" />
                    </div>
                    <span className="font-medium text-gray-800 text-lg">
                      {option.name}
                    </span>
                  </motion.li>
                );
              })}
            </ul>
          </nav>
        </div>
      </section>
    </div>
  );
};

export default BiteAILanding;
