import React, { useState, useEffect } from 'react';
import { Genders, ActivityLevels } from '../../models/calorie';
import { DietTypes } from '../../models/meals';
import { CreateUserProfile, Allergies, FoodDislikes } from '../../models/profile';
import { useProfile } from '../../hooks/useProfile';
import Input from '../common/Input';
import Select, { SelectOption } from '../common/Select';
import MultiSelect, { MultiSelectOption } from '../common/MultiSelect';

interface ProfileFormProps {
  mode: 'create' | 'edit';
  onSuccess?: () => void;
  onCancel?: () => void;
}

const ProfileForm: React.FC<ProfileFormProps> = ({ mode, onSuccess, onCancel }) => {
  const { profile, createProfile, updateProfile, loading, error } = useProfile();

  const [formData, setFormData] = useState<CreateUserProfile>({
    gender: Genders.Male,
    age: 0,
    weightInKg: 0,
    heightInCm: 0,
    activityLevel: ActivityLevels.Sedentary,
    preferredDietType: undefined,
    allergies: [],
    foodDislikes: [],
  });

  const [formErrors, setFormErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (mode === 'edit' && profile) {
      setFormData({
        gender: profile.gender,
        age: profile.age,
        weightInKg: profile.weightInKg,
        heightInCm: profile.heightInCm,
        activityLevel: profile.activityLevel,
        preferredDietType: profile.preferredDietType,
        allergies: profile.allergies || [],
        foodDislikes: profile.foodDislikes || [],
      });
    }
  }, [mode, profile]);

  const genderOptions: SelectOption[] = [
    { value: Genders.Male, label: 'Male' },
    { value: Genders.Female, label: 'Female' },
  ];

  const activityOptions: SelectOption[] = [
    { value: ActivityLevels.Sedentary, label: 'Sedentary (little to no exercise)' },
    { value: ActivityLevels.LightlyActive, label: 'Lightly Active (light exercise 1-3 days/week)' },
    { value: ActivityLevels.ModeratelyActive, label: 'Moderately Active (moderate exercise 3-5 days/week)' },
    { value: ActivityLevels.VeryActive, label: 'Very Active (hard exercise 6-7 days/week)' },
    { value: ActivityLevels.ExtraActive, label: 'Extra Active (very hard exercise & physical job)' },
  ];

  const dietOptions: SelectOption[] = [
    { value: DietTypes.Standard, label: 'Standard' },
    { value: DietTypes.Vegetarian, label: 'Vegetarian' },
    { value: DietTypes.Vegan, label: 'Vegan' },
    { value: DietTypes.Keto, label: 'Keto' },
    { value: DietTypes.LowCarb, label: 'Low Carb' },
    { value: DietTypes.Paleo, label: 'Paleo' },
    { value: DietTypes.Mediterranean, label: 'Mediterranean' },
    { value: DietTypes.GlutenFree, label: 'Gluten Free' },
    { value: DietTypes.DairyFree, label: 'Dairy Free' },
  ];

  const allergyOptions: MultiSelectOption[] = [
    { value: Allergies.Peanuts, label: 'Peanuts' },
    { value: Allergies.TreeNuts, label: 'Tree Nuts' },
    { value: Allergies.Dairy, label: 'Dairy' },
    { value: Allergies.Eggs, label: 'Eggs' },
    { value: Allergies.Soy, label: 'Soy' },
    { value: Allergies.Wheat, label: 'Wheat' },
    { value: Allergies.Gluten, label: 'Gluten' },
    { value: Allergies.Fish, label: 'Fish' },
    { value: Allergies.Shellfish, label: 'Shellfish' },
    { value: Allergies.Sesame, label: 'Sesame' },
    { value: Allergies.Mustard, label: 'Mustard' },
    { value: Allergies.Celery, label: 'Celery' },
    { value: Allergies.Sulphites, label: 'Sulphites' },
    { value: Allergies.Lupin, label: 'Lupin' },
    { value: Allergies.Molluscs, label: 'Molluscs' },
    { value: Allergies.Crustaceans, label: 'Crustaceans' },
    { value: Allergies.Milk, label: 'Milk' },
    { value: Allergies.Cheese, label: 'Cheese' },
    { value: Allergies.Yogurt, label: 'Yogurt' },
    { value: Allergies.Butter, label: 'Butter' },
    { value: Allergies.Almonds, label: 'Almonds' },
    { value: Allergies.Walnuts, label: 'Walnuts' },
    { value: Allergies.Cashews, label: 'Cashews' },
    { value: Allergies.Pistachios, label: 'Pistachios' },
    { value: Allergies.Hazelnuts, label: 'Hazelnuts' },
    { value: Allergies.BrazilNuts, label: 'Brazil Nuts' },
    { value: Allergies.MacadamiaNuts, label: 'Macadamia Nuts' },
    { value: Allergies.Pecans, label: 'Pecans' },
    { value: Allergies.PineNuts, label: 'Pine Nuts' },
    { value: Allergies.Coconut, label: 'Coconut' },
    { value: Allergies.Kiwi, label: 'Kiwi' },
    { value: Allergies.Banana, label: 'Banana' },
    { value: Allergies.Avocado, label: 'Avocado' },
    { value: Allergies.Pineapple, label: 'Pineapple' },
    { value: Allergies.Mango, label: 'Mango' },
    { value: Allergies.Papaya, label: 'Papaya' },
    { value: Allergies.PassionFruit, label: 'Passion Fruit' },
    { value: Allergies.Guava, label: 'Guava' },
    { value: Allergies.DragonFruit, label: 'Dragon Fruit' },
    { value: Allergies.Durian, label: 'Durian' },
    { value: Allergies.Jackfruit, label: 'Jackfruit' },
    { value: Allergies.Rambutan, label: 'Rambutan' },
    { value: Allergies.Lychee, label: 'Lychee' },
    { value: Allergies.Longan, label: 'Longan' },
    { value: Allergies.Mangosteen, label: 'Mangosteen' },
    { value: Allergies.Salak, label: 'Salak' },
    { value: Allergies.SnakeFruit, label: 'Snake Fruit' },
    { value: Allergies.CustardApple, label: 'Custard Apple' },
    { value: Allergies.Soursop, label: 'Soursop' },
    { value: Allergies.Atemoya, label: 'Atemoya' },
  ];

  const foodDislikeOptions: MultiSelectOption[] = [
    { value: FoodDislikes.Broccoli, label: 'Broccoli' },
    { value: FoodDislikes.BrusselsSprouts, label: 'Brussels Sprouts' },
    { value: FoodDislikes.Cauliflower, label: 'Cauliflower' },
    { value: FoodDislikes.Cabbage, label: 'Cabbage' },
    { value: FoodDislikes.Kale, label: 'Kale' },
    { value: FoodDislikes.Spinach, label: 'Spinach' },
    { value: FoodDislikes.Asparagus, label: 'Asparagus' },
    { value: FoodDislikes.Artichokes, label: 'Artichokes' },
    { value: FoodDislikes.Beets, label: 'Beets' },
    { value: FoodDislikes.Radishes, label: 'Radishes' },
    { value: FoodDislikes.Turnips, label: 'Turnips' },
    { value: FoodDislikes.Parsnips, label: 'Parsnips' },
    { value: FoodDislikes.Rutabagas, label: 'Rutabagas' },
    { value: FoodDislikes.Celery, label: 'Celery' },
    { value: FoodDislikes.Fennel, label: 'Fennel' },
    { value: FoodDislikes.Leeks, label: 'Leeks' },
    { value: FoodDislikes.Onions, label: 'Onions' },
    { value: FoodDislikes.Garlic, label: 'Garlic' },
    { value: FoodDislikes.Shallots, label: 'Shallots' },
    { value: FoodDislikes.Scallions, label: 'Scallions' },
    { value: FoodDislikes.Chives, label: 'Chives' },
    { value: FoodDislikes.Mushrooms, label: 'Mushrooms' },
    { value: FoodDislikes.Eggplant, label: 'Eggplant' },
    { value: FoodDislikes.Zucchini, label: 'Zucchini' },
    { value: FoodDislikes.Squash, label: 'Squash' },
    { value: FoodDislikes.Cucumber, label: 'Cucumber' },
    { value: FoodDislikes.BellPeppers, label: 'Bell Peppers' },
    { value: FoodDislikes.ChiliPeppers, label: 'Chili Peppers' },
    { value: FoodDislikes.Jalapenos, label: 'Jalapenos' },
    { value: FoodDislikes.Okra, label: 'Okra' },
    { value: FoodDislikes.GreenBeans, label: 'Green Beans' },
    { value: FoodDislikes.Peas, label: 'Peas' },
    { value: FoodDislikes.LimaBeans, label: 'Lima Beans' },
    { value: FoodDislikes.KidneyBeans, label: 'Kidney Beans' },
    { value: FoodDislikes.BlackBeans, label: 'Black Beans' },
    { value: FoodDislikes.PintoBeans, label: 'Pinto Beans' },
    { value: FoodDislikes.Chickpeas, label: 'Chickpeas' },
    { value: FoodDislikes.Lentils, label: 'Lentils' },
    { value: FoodDislikes.Tofu, label: 'Tofu' },
    { value: FoodDislikes.Tempeh, label: 'Tempeh' },
    { value: FoodDislikes.Seitan, label: 'Seitan' },
    { value: FoodDislikes.Quinoa, label: 'Quinoa' },
    { value: FoodDislikes.Couscous, label: 'Couscous' },
    { value: FoodDislikes.Bulgur, label: 'Bulgur' },
    { value: FoodDislikes.Farro, label: 'Farro' },
    { value: FoodDislikes.Barley, label: 'Barley' },
    { value: FoodDislikes.Rye, label: 'Rye' },
    { value: FoodDislikes.Oats, label: 'Oats' },
    { value: FoodDislikes.Millet, label: 'Millet' },
    { value: FoodDislikes.Amaranth, label: 'Amaranth' },
    { value: FoodDislikes.Buckwheat, label: 'Buckwheat' },
    { value: FoodDislikes.Sorghum, label: 'Sorghum' },
    { value: FoodDislikes.Teff, label: 'Teff' },
    { value: FoodDislikes.Spelt, label: 'Spelt' },
    { value: FoodDislikes.Kamut, label: 'Kamut' },
    { value: FoodDislikes.Einkorn, label: 'Einkorn' },
    { value: FoodDislikes.Emmer, label: 'Emmer' },
    { value: FoodDislikes.Freekeh, label: 'Freekeh' },
    { value: FoodDislikes.WildRice, label: 'Wild Rice' },
    { value: FoodDislikes.BrownRice, label: 'Brown Rice' },
    { value: FoodDislikes.WhiteRice, label: 'White Rice' },
    { value: FoodDislikes.BasmatiRice, label: 'Basmati Rice' },
    { value: FoodDislikes.JasmineRice, label: 'Jasmine Rice' },
    { value: FoodDislikes.ArborioRice, label: 'Arborio Rice' },
    { value: FoodDislikes.SushiRice, label: 'Sushi Rice' },
    { value: FoodDislikes.BlackRice, label: 'Black Rice' },
    { value: FoodDislikes.RedRice, label: 'Red Rice' },
    { value: FoodDislikes.GlutinousRice, label: 'Glutinous Rice' },
    { value: FoodDislikes.ParboiledRice, label: 'Parboiled Rice' },
    { value: FoodDislikes.ConvertedRice, label: 'Converted Rice' },
    { value: FoodDislikes.Beef, label: 'Beef' },
    { value: FoodDislikes.Pork, label: 'Pork' },
    { value: FoodDislikes.Chicken, label: 'Chicken' },
    { value: FoodDislikes.Turkey, label: 'Turkey' },
    { value: FoodDislikes.Duck, label: 'Duck' },
    { value: FoodDislikes.Goose, label: 'Goose' },
    { value: FoodDislikes.Lamb, label: 'Lamb' },
    { value: FoodDislikes.Veal, label: 'Veal' },
    { value: FoodDislikes.Rabbit, label: 'Rabbit' },
    { value: FoodDislikes.Venison, label: 'Venison' },
    { value: FoodDislikes.Bison, label: 'Bison' },
    { value: FoodDislikes.Elk, label: 'Elk' },
    { value: FoodDislikes.Ostrich, label: 'Ostrich' },
    { value: FoodDislikes.Emu, label: 'Emu' },
    { value: FoodDislikes.Quail, label: 'Quail' },
    { value: FoodDislikes.Pheasant, label: 'Pheasant' },
    { value: FoodDislikes.CornishHen, label: 'Cornish Hen' },
    { value: FoodDislikes.Squab, label: 'Squab' },
    { value: FoodDislikes.Salmon, label: 'Salmon' },
    { value: FoodDislikes.Tuna, label: 'Tuna' },
    { value: FoodDislikes.Cod, label: 'Cod' },
    { value: FoodDislikes.Halibut, label: 'Halibut' },
    { value: FoodDislikes.Swordfish, label: 'Swordfish' },
    { value: FoodDislikes.MahiMahi, label: 'Mahi Mahi' },
    { value: FoodDislikes.Grouper, label: 'Grouper' },
    { value: FoodDislikes.Snapper, label: 'Snapper' },
    { value: FoodDislikes.Flounder, label: 'Flounder' },
    { value: FoodDislikes.Sole, label: 'Sole' },
    { value: FoodDislikes.Trout, label: 'Trout' },
    { value: FoodDislikes.Catfish, label: 'Catfish' },
    { value: FoodDislikes.Tilapia, label: 'Tilapia' },
    { value: FoodDislikes.Shrimp, label: 'Shrimp' },
    { value: FoodDislikes.Crab, label: 'Crab' },
    { value: FoodDislikes.Lobster, label: 'Lobster' },
    { value: FoodDislikes.Clams, label: 'Clams' },
    { value: FoodDislikes.Mussels, label: 'Mussels' },
    { value: FoodDislikes.Oysters, label: 'Oysters' },
    { value: FoodDislikes.Scallops, label: 'Scallops' },
    { value: FoodDislikes.Squid, label: 'Squid' },
    { value: FoodDislikes.Octopus, label: 'Octopus' },
    { value: FoodDislikes.Calamari, label: 'Calamari' },
    { value: FoodDislikes.SeaUrchin, label: 'Sea Urchin' },
    { value: FoodDislikes.Roe, label: 'Roe' },
    { value: FoodDislikes.Caviar, label: 'Caviar' },
    { value: FoodDislikes.Anchovies, label: 'Anchovies' },
    { value: FoodDislikes.Sardines, label: 'Sardines' },
    { value: FoodDislikes.Herring, label: 'Herring' },
    { value: FoodDislikes.Mackerel, label: 'Mackerel' },
    { value: FoodDislikes.Bluefish, label: 'Bluefish' },
    { value: FoodDislikes.Monkfish, label: 'Monkfish' },
    { value: FoodDislikes.Skate, label: 'Skate' },
    { value: FoodDislikes.Eel, label: 'Eel' },
    { value: FoodDislikes.Conch, label: 'Conch' },
    { value: FoodDislikes.Abalone, label: 'Abalone' },
    { value: FoodDislikes.Milk, label: 'Milk' },
    { value: FoodDislikes.Cheese, label: 'Cheese' },
    { value: FoodDislikes.Yogurt, label: 'Yogurt' },
    { value: FoodDislikes.Butter, label: 'Butter' },
    { value: FoodDislikes.Cream, label: 'Cream' },
    { value: FoodDislikes.IceCream, label: 'Ice Cream' },
    { value: FoodDislikes.CottageCheese, label: 'Cottage Cheese' },
    { value: FoodDislikes.Ricotta, label: 'Ricotta' },
    { value: FoodDislikes.Mozzarella, label: 'Mozzarella' },
    { value: FoodDislikes.Cheddar, label: 'Cheddar' },
    { value: FoodDislikes.Parmesan, label: 'Parmesan' },
    { value: FoodDislikes.Gouda, label: 'Gouda' },
    { value: FoodDislikes.Brie, label: 'Brie' },
    { value: FoodDislikes.Camembert, label: 'Camembert' },
    { value: FoodDislikes.Roquefort, label: 'Roquefort' },
    { value: FoodDislikes.Feta, label: 'Feta' },
    { value: FoodDislikes.GoatCheese, label: 'Goat Cheese' },
    { value: FoodDislikes.BlueCheese, label: 'Blue Cheese' },
    { value: FoodDislikes.Swiss, label: 'Swiss' },
    { value: FoodDislikes.Provolone, label: 'Provolone' },
    { value: FoodDislikes.MontereyJack, label: 'Monterey Jack' },
    { value: FoodDislikes.Colby, label: 'Colby' },
    { value: FoodDislikes.American, label: 'American' },
    { value: FoodDislikes.Velveeta, label: 'Velveeta' },
    { value: FoodDislikes.Eggs, label: 'Eggs' },
    { value: FoodDislikes.Mayonnaise, label: 'Mayonnaise' },
    { value: FoodDislikes.Aioli, label: 'Aioli' },
    { value: FoodDislikes.TartarSauce, label: 'Tartar Sauce' },
    { value: FoodDislikes.Hollandaise, label: 'Hollandaise' },
    { value: FoodDislikes.Bearnaise, label: 'Bearnaise' },
    { value: FoodDislikes.CaesarDressing, label: 'Caesar Dressing' },
    { value: FoodDislikes.RanchDressing, label: 'Ranch Dressing' },
    { value: FoodDislikes.ThousandIsland, label: 'Thousand Island' },
    { value: FoodDislikes.ItalianDressing, label: 'Italian Dressing' },
    { value: FoodDislikes.FrenchDressing, label: 'French Dressing' },
    { value: FoodDislikes.BalsamicVinaigrette, label: 'Balsamic Vinaigrette' },
    { value: FoodDislikes.OilAndVinegar, label: 'Oil and Vinegar' },
    { value: FoodDislikes.HoneyMustard, label: 'Honey Mustard' },
    { value: FoodDislikes.BlueCheeseDressing, label: 'Blue Cheese Dressing' },
    { value: FoodDislikes.RussianDressing, label: 'Russian Dressing' },
    { value: FoodDislikes.SesameGinger, label: 'Sesame Ginger' },
    { value: FoodDislikes.Teriyaki, label: 'Teriyaki' },
    { value: FoodDislikes.SoySauce, label: 'Soy Sauce' },
    { value: FoodDislikes.Worcestershire, label: 'Worcestershire' },
    { value: FoodDislikes.HotSauce, label: 'Hot Sauce' },
    { value: FoodDislikes.Sriracha, label: 'Sriracha' },
    { value: FoodDislikes.Tabasco, label: 'Tabasco' },
    { value: FoodDislikes.BuffaloSauce, label: 'Buffalo Sauce' },
    { value: FoodDislikes.BBQ, label: 'BBQ' },
    { value: FoodDislikes.Ketchup, label: 'Ketchup' },
    { value: FoodDislikes.Mustard, label: 'Mustard' },
    { value: FoodDislikes.Relish, label: 'Relish' },
    { value: FoodDislikes.Pickles, label: 'Pickles' },
    { value: FoodDislikes.Olives, label: 'Olives' },
    { value: FoodDislikes.Capers, label: 'Capers' },
    { value: FoodDislikes.Horseradish, label: 'Horseradish' },
    { value: FoodDislikes.Wasabi, label: 'Wasabi' },
    { value: FoodDislikes.Apples, label: 'Apples' },
    { value: FoodDislikes.Bananas, label: 'Bananas' },
    { value: FoodDislikes.Oranges, label: 'Oranges' },
    { value: FoodDislikes.Grapes, label: 'Grapes' },
    { value: FoodDislikes.Strawberries, label: 'Strawberries' },
    { value: FoodDislikes.Blueberries, label: 'Blueberries' },
    { value: FoodDislikes.Raspberries, label: 'Raspberries' },
    { value: FoodDislikes.Blackberries, label: 'Blackberries' },
    { value: FoodDislikes.Cranberries, label: 'Cranberries' },
    { value: FoodDislikes.Pineapple, label: 'Pineapple' },
    { value: FoodDislikes.Mango, label: 'Mango' },
    { value: FoodDislikes.Papaya, label: 'Papaya' },
    { value: FoodDislikes.Kiwi, label: 'Kiwi' },
    { value: FoodDislikes.PassionFruit, label: 'Passion Fruit' },
    { value: FoodDislikes.Guava, label: 'Guava' },
    { value: FoodDislikes.DragonFruit, label: 'Dragon Fruit' },
    { value: FoodDislikes.Durian, label: 'Durian' },
    { value: FoodDislikes.Jackfruit, label: 'Jackfruit' },
    { value: FoodDislikes.Rambutan, label: 'Rambutan' },
    { value: FoodDislikes.Lychee, label: 'Lychee' },
    { value: FoodDislikes.Longan, label: 'Longan' },
    { value: FoodDislikes.Mangosteen, label: 'Mangosteen' },
    { value: FoodDislikes.Salak, label: 'Salak' },
    { value: FoodDislikes.SnakeFruit, label: 'Snake Fruit' },
    { value: FoodDislikes.CustardApple, label: 'Custard Apple' },
    { value: FoodDislikes.Soursop, label: 'Soursop' },
    { value: FoodDislikes.Atemoya, label: 'Atemoya' },
    { value: FoodDislikes.Cherries, label: 'Cherries' },
    { value: FoodDislikes.Pears, label: 'Peaches' },
    { value: FoodDislikes.Plums, label: 'Plums' },
    { value: FoodDislikes.Apricots, label: 'Apricots' },
    { value: FoodDislikes.Nectarines, label: 'Nectarines' },
    { value: FoodDislikes.Pears, label: 'Pears' },
    { value: FoodDislikes.Lemons, label: 'Lemons' },
    { value: FoodDislikes.Limes, label: 'Limes' },
    { value: FoodDislikes.Grapefruit, label: 'Grapefruit' },
    { value: FoodDislikes.Tangerines, label: 'Tangerines' },
    { value: FoodDislikes.Clementines, label: 'Clementines' },
    { value: FoodDislikes.Mandarins, label: 'Mandarins' },
    { value: FoodDislikes.Pomelo, label: 'Pomelo' },
    { value: FoodDislikes.Kumquats, label: 'Kumquats' },
    { value: FoodDislikes.Calamansi, label: 'Calamansi' },
    { value: FoodDislikes.Yuzu, label: 'Yuzu' },
    { value: FoodDislikes.BuddhaHand, label: 'Buddha Hand' },
    { value: FoodDislikes.FingerLimes, label: 'Finger Limes' },
    { value: FoodDislikes.Chocolate, label: 'Chocolate' },
    { value: FoodDislikes.DarkChocolate, label: 'Dark Chocolate' },
    { value: FoodDislikes.MilkChocolate, label: 'Milk Chocolate' },
    { value: FoodDislikes.WhiteChocolate, label: 'White Chocolate' },
    { value: FoodDislikes.Cocoa, label: 'Cocoa' },
    { value: FoodDislikes.Carob, label: 'Carob' },
    { value: FoodDislikes.Licorice, label: 'Licorice' },
    { value: FoodDislikes.Peppermint, label: 'Peppermint' },
    { value: FoodDislikes.Spearmint, label: 'Spearmint' },
    { value: FoodDislikes.Cinnamon, label: 'Cinnamon' },
    { value: FoodDislikes.Nutmeg, label: 'Nutmeg' },
    { value: FoodDislikes.Cloves, label: 'Cloves' },
    { value: FoodDislikes.Allspice, label: 'Allspice' },
    { value: FoodDislikes.Cardamom, label: 'Cardamom' },
    { value: FoodDislikes.Coriander, label: 'Coriander' },
    { value: FoodDislikes.Cumin, label: 'Cumin' },
    { value: FoodDislikes.FennelSeeds, label: 'Fennel Seeds' },
    { value: FoodDislikes.Fenugreek, label: 'Fenugreek' },
    { value: FoodDislikes.MustardSeeds, label: 'Mustard Seeds' },
    { value: FoodDislikes.PoppySeeds, label: 'Poppy Seeds' },
    { value: FoodDislikes.SesameSeeds, label: 'Sesame Seeds' },
    { value: FoodDislikes.SunflowerSeeds, label: 'Sunflower Seeds' },
    { value: FoodDislikes.PumpkinSeeds, label: 'Pumpkin Seeds' },
    { value: FoodDislikes.ChiaSeeds, label: 'Chia Seeds' },
    { value: FoodDislikes.Flaxseeds, label: 'Flaxseeds' },
    { value: FoodDislikes.HempSeeds, label: 'Hemp Seeds' },
    { value: FoodDislikes.PineNuts, label: 'Pine Nuts' },
    { value: FoodDislikes.Almonds, label: 'Almonds' },
    { value: FoodDislikes.Walnuts, label: 'Walnuts' },
    { value: FoodDislikes.Cashews, label: 'Cashews' },
    { value: FoodDislikes.Pistachios, label: 'Pistachios' },
    { value: FoodDislikes.Hazelnuts, label: 'Hazelnuts' },
    { value: FoodDislikes.BrazilNuts, label: 'Brazil Nuts' },
    { value: FoodDislikes.MacadamiaNuts, label: 'Macadamia Nuts' },
    { value: FoodDislikes.Pecans, label: 'Pecans' },
    { value: FoodDislikes.Chestnuts, label: 'Chestnuts' },
    { value: FoodDislikes.Coconut, label: 'Coconut' },
    { value: FoodDislikes.Peanuts, label: 'Peanuts' },
    { value: FoodDislikes.Salt, label: 'Salt' },
    { value: FoodDislikes.Pepper, label: 'Pepper' },
    { value: FoodDislikes.Paprika, label: 'Paprika' },
    { value: FoodDislikes.ChiliPowder, label: 'Chili Powder' },
    { value: FoodDislikes.CurryPowder, label: 'Curry Powder' },
    { value: FoodDislikes.GaramMasala, label: 'Garam Masala' },
    { value: FoodDislikes.TacoSeasoning, label: 'Taco Seasoning' },
    { value: FoodDislikes.ItalianSeasoning, label: 'Italian Seasoning' },
    { value: FoodDislikes.HerbsDeProvence, label: 'Herbs de Provence' },
    { value: FoodDislikes.PoultrySeasoning, label: 'Poultry Seasoning' },
    { value: FoodDislikes.PumpkinPieSpice, label: 'Pumpkin Pie Spice' },
    { value: FoodDislikes.ApplePieSpice, label: 'Apple Pie Spice' },
    { value: FoodDislikes.ChineseFiveSpice, label: 'Chinese Five Spice' },
    { value: FoodDislikes.CajunSeasoning, label: 'Cajun Seasoning' },
    { value: FoodDislikes.CreoleSeasoning, label: 'Creole Seasoning' },
    { value: FoodDislikes.OldBay, label: 'Old Bay' },
    { value: FoodDislikes.LemonPepper, label: 'Lemon Pepper' },
    { value: FoodDislikes.GarlicPowder, label: 'Garlic Powder' },
    { value: FoodDislikes.OnionPowder, label: 'Onion Powder' },
    { value: FoodDislikes.Thyme, label: 'Thyme' },
    { value: FoodDislikes.Rosemary, label: 'Rosemary' },
    { value: FoodDislikes.Sage, label: 'Sage' },
    { value: FoodDislikes.Oregano, label: 'Oregano' },
    { value: FoodDislikes.Basil, label: 'Basil' },
    { value: FoodDislikes.Parsley, label: 'Parsley' },
    { value: FoodDislikes.Cilantro, label: 'Cilantro' },
    { value: FoodDislikes.Dill, label: 'Dill' },
    { value: FoodDislikes.Tarragon, label: 'Tarragon' },
    { value: FoodDislikes.Chervil, label: 'Chervil' },
    { value: FoodDislikes.Sorrel, label: 'Sorrel' },
    { value: FoodDislikes.Borage, label: 'Borage' },
    { value: FoodDislikes.Lovage, label: 'Lovage' },
    { value: FoodDislikes.SummerSavory, label: 'Summer Savory' },
    { value: FoodDislikes.WinterSavory, label: 'Winter Savory' },
    { value: FoodDislikes.Marjoram, label: 'Marjoram' },
    { value: FoodDislikes.Hyssop, label: 'Hyssop' },
  ];

  const validateForm = (): boolean => {
    const errors: Record<string, string> = {};

    if (formData.age <= 0 || formData.age > 120) {
      errors.age = 'Please enter a valid age (1-120)';
    }
    if (formData.weightInKg <= 0 || formData.weightInKg > 500) {
      errors.weightInKg = 'Please enter a valid weight (1-500 kg)';
    }
    if (formData.heightInCm <= 0 || formData.heightInCm > 250) {
      errors.heightInCm = 'Please enter a valid height (1-250 cm)';
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) return;

    const data = formData;

    let result;
    if (mode === 'create') {
      result = await createProfile(data);
    } else {
      result = await updateProfile(data);
    }

    if (result.success) {
      onSuccess?.();
    }
  };

  const handleInputChange = (field: keyof CreateUserProfile, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    if (formErrors[field]) {
      setFormErrors(prev => ({ ...prev, [field]: '' }));
    }
  };

  return (
    <div className="max-w-md mx-auto bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-6 text-center">
        {mode === 'create' ? 'Create Your Profile' : 'Edit Profile'}
      </h2>

      {error && (
        <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <Select
          id="gender"
          label="Gender"
          value={formData.gender.toString()}
          onChange={(e) => handleInputChange('gender', parseInt(e.target.value))}
          options={genderOptions}
          required
        />

        <Input
          id="age"
          type="number"
          label="Age"
          value={formData.age.toString()}
          onChange={(e) => handleInputChange('age', parseInt(e.target.value) || 0)}
          placeholder="Enter your age"
          error={formErrors.age}
          required
        />

        <Input
          id="weight"
          type="number"
          label="Weight (kg)"
          value={formData.weightInKg.toString()}
          onChange={(e) => handleInputChange('weightInKg', parseFloat(e.target.value) || 0)}
          placeholder="Enter your weight in kg"
          error={formErrors.weightInKg}
          required
        />

        <Input
          id="height"
          type="number"
          label="Height (cm)"
          value={formData.heightInCm.toString()}
          onChange={(e) => handleInputChange('heightInCm', parseInt(e.target.value) || 0)}
          placeholder="Enter your height in cm"
          error={formErrors.heightInCm}
          required
        />

        <Select
          id="activity"
          label="Activity Level"
          value={formData.activityLevel.toString()}
          onChange={(e) => handleInputChange('activityLevel', parseInt(e.target.value))}
          options={activityOptions}
          required
        />

        <Select
          id="diet"
          label="Preferred Diet Type (Optional)"
          value={formData.preferredDietType?.toString() || ''}
          onChange={(e) => handleInputChange('preferredDietType', e.target.value ? parseInt(e.target.value) : undefined)}
          options={dietOptions}
        />

        <MultiSelect
          id="allergies"
          label="Allergies (Optional)"
          value={formData.allergies?.map(a => a) || []}
          onChange={(values) => handleInputChange('allergies', values)}
          options={allergyOptions}
          placeholder="Select your allergies..."
        />

        <MultiSelect
          id="dislikes"
          label="Food Dislikes (Optional)"
          value={formData.foodDislikes?.map(d => d) || []}
          onChange={(values) => handleInputChange('foodDislikes', values)}
          options={foodDislikeOptions}
          placeholder="Select foods you dislike..."
        />

        <div className="flex space-x-4 pt-4">
          <button
            type="submit"
            disabled={loading}
            className="flex-1 bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 disabled:opacity-50"
          >
            {loading ? 'Saving...' : mode === 'create' ? 'Create Profile' : 'Update Profile'}
          </button>
          {onCancel && (
            <button
              type="button"
              onClick={onCancel}
              className="flex-1 bg-gray-300 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-400"
            >
              Cancel
            </button>
          )}
        </div>
      </form>
    </div>
  );
};

export default ProfileForm;