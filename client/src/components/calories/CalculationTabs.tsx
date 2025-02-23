import { Tab, TabGroup, TabList } from "@headlessui/react";
import React from "react";

interface TabsProps {
  calculationType: "goals" | "target";
  setCalculationType: (type: "goals" | "target") => void;
}

const CalculationTabs = ({ calculationType, setCalculationType }: TabsProps) => {
  return (
    <TabGroup
      selectedIndex={calculationType === "goals" ? 0 : 1}
      onChange={(index) => setCalculationType(index === 0 ? "goals" : "target")}
    >
      <TabList className="flex p-1 mb-5 space-x-1 bg-gray-200 rounded-xl">
        <Tab
          className={({ selected }) =>
            `w-full py-2.5 text-sm font-medium leading-5 rounded-lg focus:outline-none
            ${
              selected
                ? "bg-pastel-green text-white shadow"
                : "text-gray-700 hover:bg-gray-100"
            } transition-all duration-200`
          }
        >
          Calculate Goals
        </Tab>
        <Tab
          className={({ selected }) =>
            `w-full py-2.5 text-sm font-medium leading-5 rounded-lg focus:outline-none
            ${
              selected
                ? "bg-pastel-green text-white shadow"
                : "text-gray-700 hover:bg-gray-100"
            } transition-all duration-200`
          }
        >
          Target Calculator
        </Tab>
      </TabList>
    </TabGroup>
  );
};

export default CalculationTabs;
