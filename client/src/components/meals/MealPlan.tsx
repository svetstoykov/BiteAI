import React, { useState } from 'react';
import { Clock } from 'lucide-react';

const MealPlan = () => {
  // State for active tab
  const [activeTab, setActiveTab] = useState('Today');
  
  // Sample meal data
  const meals = [
    {
      time: '8:00 AM',
      items: 'Oatmeal, Milk',
      type: 'Breakfast'
    },
    {
      time: '12:00 PM',
      items: 'Sandwich, Apple',
      type: 'Lunch'
    },
    {
      time: '6:00 PM',
      items: 'Pasta, Salad',
      type: 'Dinner'
    }
  ];
  
  // Nutrition data
  const nutritionData = [
    { name: 'Calories', value: '2,000' },
    { name: 'Protein', value: '80g' },
    { name: 'Carbs', value: '200g' },
    { name: 'Fat', value: '100g' },
    { name: 'Fiber', value: '25g' }
  ];

  return (
    <div className="max-w-2xl mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Meal Plan</h1>
      
      {/* Tab Navigation */}
      <div className="flex border-b mb-6">
        {['Today', 'Tomorrow', 'Add a Meal'].map((tab) => (
          <button
            key={tab}
            className={`pb-2 px-4 ${
              activeTab === tab 
                ? 'border-b-2 border-black font-medium' 
                : 'text-gray-500'
            }`}
            onClick={() => setActiveTab(tab)}
          >
            {tab}
          </button>
        ))}
      </div>
      
      {/* Meal Cards */}
      <div className="mb-8">
        {meals.map((meal, index) => (
          <div key={index} className="flex items-center p-4 bg-gray-50 rounded-lg mb-4">
            <div className="mr-4">
              <Clock size={20} className="text-gray-500" />
            </div>
            <div className="flex-1">
              <div className="font-medium">{meal.time}</div>
              <div className="text-gray-500">{meal.items}</div>
              <div className="text-gray-500 text-sm">{meal.type}</div>
            </div>
            <button className="text-sm">Edit</button>
          </div>
        ))}
      </div>
      
      {/* Bullet List */}
      <div className="mb-8">
        {meals.map((meal, index) => (
          <div key={index} className="flex mb-4 pl-2">
            <div className="mr-2 mt-1">•</div>
            <div>
              <div className="font-medium">{meal.time}</div>
              <div className="text-gray-500">{meal.items}</div>
            </div>
          </div>
        ))}
      </div>
      
      {/* Generate Button */}
      <div className="flex justify-center mb-8">
        <button className="bg-gray-100 text-gray-800 px-4 py-2 rounded">
          Generate Meal Plan
        </button>
      </div>
      
      {/* Nutrition Summary */}
      <h2 className="text-xl font-bold mb-4 pb-2 border-b">Nutrition Summary</h2>
      <div>
        {nutritionData.map((item, index) => (
          <div key={index} className="flex py-4 border-b">
            <div className="flex-1 text-gray-500">{item.name}</div>
            <div className="font-medium">{item.value}</div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default MealPlan;