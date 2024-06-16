using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Viguar.Aircraft;

namespace Viguar.WeatherDynamics
{
public class weatherCalculator : MonoBehaviour
{
    //External Variables
    private weatherZone m_Zone;

    //Enumerations
    [HideInInspector] public enum WeatherDynamicsTypes               //The types of weather dynamics that can occur.
    {
        ClearCold,                                      //Atmospheric Pressure above 1020hPa, Temperature below 0°C, Humidity is low.
        Normal,                                         //If no other conditions are met.
        BrokenClouds,                                   //Cloud coverage is moderate and the sky is partially clear.
        Overcast,                                       //Cloud coverage is extensive and there is no significant precipitation.
        Thunderstorm,                                   //Atmoshperic Pressure is below 980 hPa, Temperature is above 15°C, Humidity is high.
        Heatwave,                                       //Atmospheric Pressure is above 1015 hPa, Temperature is above 32°C, Humidity is high.
        Stormy,                                         //
        Blizzard,                                       //Atmospheric Pressure is below 950 hPa, Temperature is below 0°C, Humidity is high.
    }
    [HideInInspector] public enum PrecipitationTypes                 //The types of precipitation that can occur. //Need to complete conditions.
    {
        None,                                           //No precipitation is present/possible.        
        Drizzle,                                        //Light Rain. Temperature needs to be between 5°C and 15°C. Humidity needs to be above 70%.   
        Rain,                                           //Rain. Temperature above 5°C. Humidity is moderate to high.
        Sleet,                                          //Sleet. Temperature at or below 5°C. Relative Humidity is above 70%.
        Snow,                                           //Snow. Temperture at or below 5°C. Relative Humidity is moderate to high.
        Hail,                                           //Hail. Temperaature above 20°C. Humidity high. 
        Meatballs,                                      //Meatball Mode Precipitation above 80°C for an extra spicy playing experience.
    }
    [HideInInspector] public enum WindTypes                          //The types of wind levels that can occur. //Need to complete conditions.
    {
        Calm,                                           //The wind is very mild. WindSpeed below 30 kp/h.
        Moderate,                                       //The wind is moderate. WindSpeed between 30 and 50 kp/h.
        FluctuatingStrong,                              //The wind is moderate, but more fluctuant and less predictable. WindSpeed between 30 and 70 kp/h.
        Storm,                                          //The wind is stormy. WindSpeed between 70 and 100 kp/h.
        Violent,                                        //The wind is violently stormy. Windspeed from 100-130 kp/h.       
    }
    [HideInInspector] public enum HumidityRanges                     //The types of defined humidity classifications.
    {
        ExtremelyDry,                                   //Relative Humidity of 15% and lower.     
        Arid,                                           //Relative Humidity of 15% - 25%.
        Moderate,                                       //Relative Humidity of 25% - 55%.
        Humid,                                          //Relative Humidity of 55% - 75%.
        Oppressive,                                     //Relative Humidity of 75% and greater.
    }
    [HideInInspector] public enum TemperatureRanges                  //The types of defined temperature classifications.
    {
        ExtremelyCold,                              //Temperature between -40 and -20
        VeryCold,                                   //Temperature between -20 and -8
        Frosty,                                     //Temperature between - 8 and 5
        Moderate,                                   //Temperature between 5 and 24
        Warm,                                       //Temperature between 24 and 32   
        VeryHot,                                    //Temperature between 32 and 48
        HazardousHot,                               //Temperature between 48 and 60
    }
    [HideInInspector] public enum PressureRanges                     //The types of defines air pressure classifications.
    {
        HuracanLow,                                         //Pressure below 950 hPa.
        StormLow,                                           //Pressure from 950 to 980 hPa.
        Low,                                                //Pressure from 980 to 1006 hPa.
        Neutral,                                            //Pressure from 1006 to 1020 hPa.
        High,                                               //Pressure from 1020 to 1035 hPa.
        VeryHigh                                            //Pressure above 1035 hPa.
    }

        //Public Weather Variable Output
        public float GlobalTemperature { get; private set; }
        public float GlobalAirpressure { get; private set; }
        public float GlobalPrecipitationIntensity { get; private set; }
        public float GlobalCloudIntensity { get; private set; }
        public float GlobalFogIntensity { get; private set; }
        public float GlobalWindIntensity { get; private set; }
        public float GlobalWindDirection { get; private set; }

        public PrecipitationTypes GlobalPrecipitationType { get; private set; }
        public bool ZoneCloudsPossible { get; private set; }
        public bool ZoneFogPossible { get; private set; }
        public bool ZonePrecipitationPossible { get; private set; }        
        


        //Weather Variables
        private int SeaLevelTemperature;                //Temperature at sea level.
    private int SeaLevelAirpressure;                //Airpressure at sea level.
    private float SeaLevelHumidity;                   //Relative humidity at sea level.
    private int SeaLevelWindSpeed;                  //Wind Speed at sea level.

    private float CloudPercentageLevel;             //The calculated amount of clouds covering the sky. 0% are no clouds, 100% is the sky completely covered.
    private float FogPercentageLevel;               //The calculated amount of fog at sea level. 0% is full visibility, at 100% lowest visibility. (control for that needs to be still thought out).
    private float WindIntensity;                    //The intensity factor for wind strength.
    private int PrecipitationIntensity;             //The intensity factor for precipitation.

    private bool Freezing;                          //Whether or not the current Sea Level Temperature is below freezing point.
    private bool CloudFormationPossible;            //Whether or not the current Weather Dynamic allows for clouds (and subsequently fog) to form.
    private bool PrecipitationPossible;             //Whether or not the current Weather Dynamic allows for precipitation to be possible.

    private WeatherDynamicsTypes currentWeatherDynamic;     //The current weather dynamic defined from the WeatherDynamicsTypes.
    private PrecipitationTypes currentPrecipitationType;    //The current type of precipitation that is possible, based on the currentWeatherDynamic.
    private WindTypes currentWindType;                      //The current type of wind that is present, based on atmospheric Pressure.    
    private HumidityRanges currentHumidityCategory;         //The current class of humidity at sea level.
    private TemperatureRanges currentTemperatureCategory;   //The current class of temperature at sea level.    
    private PressureRanges currentPressureCategory;            //The current class of pressure Range at sea level.

    //Clamp Limits
    private int MinimumTemperature = -40;           //The lowest possible air temperature at sea level.
    private int MaximumTemperature = 60;            //The highest possible air temperature at sea level.
    private int MinimumAirpressure = 870;           //The lowest possible air pressure at sea level (870 is the world record of lowest ever sea level air pressure).
    private int MaximumAirpressure = 1084;          //The highest possible air pressure at sea level (1084 is the world record of highest ever sea level air pressure).
    private int MinimumHumidity = 1;                //The lowest possible relative humidity at sea level.
    private int MaximumHumidity = 100;              //The highest possible relative humidity at sea level.
    private int MinimumWindSpeed = 0;               //The slowest possible speed for Wind in kp/h.
    private int MaximumWindSpeed = 118;             //The fasted possible speed for Wind in kp/h (118 is the indicator for max. wind speed on most wind scales).

    void Start()
    {
            PrecipitationPossible = false;
            m_Zone = GetComponent<weatherZone>();           
    }
   
    private void setGlobalValues()
    {
           GlobalCloudIntensity = CloudPercentageLevel;
           ZoneCloudsPossible = CloudFormationPossible;
           ZoneFogPossible = CloudFormationPossible;
           GlobalFogIntensity = FogPercentageLevel;
           GlobalPrecipitationType = currentPrecipitationType;
           ZonePrecipitationPossible = PrecipitationPossible;      
           
        }

    #region Generating Global Weather

    public void generateGlobalWeather(int temp, int press, int hum)
    {
        SeaLevelTemperature = temp;
        SeaLevelAirpressure = press;
        SeaLevelHumidity = hum;            
            
        weatherCategoryConditionals();        
        determineCurrentWeather();        
        setGlobalValues();
        setGlobalValues();
        //print("|| Zone Dynamic: " + currentWeatherDynamic + " || Zone Clouds: " + CloudPercentageLevel + " || Zone Precipitation: " + currentPrecipitationType + " || Zone Wind: " + currentWindType + " || Temp. Cat.: " + currentTemperatureCategory + " || Press. Cat.: " + currentPressureCategory + " || Hum. Cat.: " + currentHumidityCategory);
    }

    private void weatherCategoryConditionals()
    {
        #region Explanation
        /*
         *  =====CONDITIONALS ORDER=====
         *  We will determine what type of global weather we have using the following logic:
         *  Please bear in mind that this logic is simplified and not neccessarily comparable to real life.
         *  
         *  [Step 1 - Temperature Category]
         *  -First, we need to look at the temperature at Sea Level, since it dictates what type of precipitation at the temperature can actually occur. (Snow, Sleet, Drizzle, Rain, Hail)
         *  -The category has been analysed in earlier functions "analyseAirTemperature()" 
         *      -If The precipitationTypes returns with "Rainfall", we will need to later run a check on its type, as there can be Drizzle, Rain or, in specific cases, Hail.
         *  
         *  [Step 2 - Pressure Category]
         *  - First, we look at the current Airpressure category. The category has been analysed in earlier functions "analyseAirPressure()"
         *      - With the help if Air Pressure, we can determine whether Drizzle, Rain or Hail may occur.
         *      - With the help of Air Pressure Category we can determine whether clouds (and fog) can form (normally clouds cannot form at high pressure)
         *          - We effectively only run the calculations for cloud formations and fog formations, if the category allows it.
         *      - With the help of Air Pressure Category we can determine the amounts of wind that will occur too.
         *          - We effectively set the wind category based on the pressure.
         *          
         *          ?? do i add this part like this..
         *  [Step 3 - Humidity Category]
         *  -Knowing what type of precipitation COULD occur at temperature and pressure, we now look at the humidity. The category has been analysed in earlier functions "analyseRelativeHumidity()" 
         *      -We do this to confirm whether or not it's actually possible to have types of precipitation in the first place.   
         *          
         *  [Step 4] -          
         *          
         */
        #endregion
        //Check Conditionals
        runHumidityConditionals();                      //Precipitation - Possible / Not Possible
        runPressureConditionals();                      //Clouds & Wind - Possible / Not Possible & Wind Intensity
        runTemperatureConditionals();                   //Precipitation - Type
        

                
        //calculateGlobalFogLevel();                      //Set fog intensity, if cloud forming is possible.
        //calculateGlobalPrecipitationIntensity();        //Set amount of precipitation, if precipitation is possible and clouds formed.
        //SwitchGlobalPrecipitation();
        //calculateGlobalWindIntensity();                 //Set wind multiplicator based on air pressure.
    }

    private void determineCurrentWeather()
    {     
        if (SeaLevelAirpressure < 1006)
        {
            if (currentPressureCategory is PressureRanges.StormLow && SeaLevelTemperature > 15 && SeaLevelHumidity > 65)
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Thunderstorm;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
            }
            else if (currentPressureCategory is PressureRanges.HuracanLow && SeaLevelTemperature < -8 && currentHumidityCategory is HumidityRanges.Oppressive)
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Blizzard;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
                }
            else
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Stormy;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
                }
        }
        else if (SeaLevelAirpressure > 1013)
        {
            if (SeaLevelAirpressure > 1015 && SeaLevelTemperature > 32 && currentHumidityCategory is HumidityRanges.Oppressive)
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Heatwave;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
                }
            else if (SeaLevelAirpressure > 1020 && SeaLevelTemperature < 0 && SeaLevelHumidity < 25 && !CloudFormationPossible)
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.ClearCold;
                    calculateGlobalFogLevel();
                    CloudPercentageLevel = 0;
                }
            else
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Normal;
                    calculateGlobalFogLevel();
                    calculateGlobalCloudLevel();
                }
        }
        else
        {
            if (SeaLevelAirpressure < 1013 && SeaLevelAirpressure > 1006 && CloudFormationPossible)
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Overcast;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
                }
            else if (SeaLevelAirpressure > 1013 && SeaLevelAirpressure < 1020 && CloudFormationPossible)
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.BrokenClouds;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
                }
            else
            {
                    currentWeatherDynamic = WeatherDynamicsTypes.Normal;
                    calculateGlobalCloudLevel();
                    calculateGlobalFogLevel();
                }
        }



            
        }

    //Weather Value Analysis
    private void analyseAirTemperature() //Sets the currentTemperatureCategory enumeration based on the temperature.
    {
        if (SeaLevelTemperature > -40 && SeaLevelTemperature <= -20) { currentTemperatureCategory = TemperatureRanges.ExtremelyCold; }
        else if (SeaLevelTemperature > -20 && SeaLevelTemperature <= -8) { currentTemperatureCategory = TemperatureRanges.VeryCold; }
        else if (SeaLevelTemperature > -8 && SeaLevelTemperature <= 5) { currentTemperatureCategory = TemperatureRanges.Frosty; }
        else if (SeaLevelTemperature > 5 && SeaLevelTemperature <= 24) { currentTemperatureCategory = TemperatureRanges.Moderate; }
        else if (SeaLevelTemperature > 24 && SeaLevelTemperature <= 32) { currentTemperatureCategory = TemperatureRanges.Warm; }
        else if (SeaLevelTemperature > 32 && SeaLevelTemperature <= 48) { currentTemperatureCategory = TemperatureRanges.VeryHot; }
        else if (SeaLevelTemperature > 48 && SeaLevelTemperature <= 60) { currentTemperatureCategory = TemperatureRanges.HazardousHot; }
    }
    private void analyseAirPressure()   //Sets the currentPressureCategory enumeration based on the air pressure.
    {
        if (SeaLevelAirpressure > 870 && SeaLevelAirpressure <= 950) { currentPressureCategory = PressureRanges.HuracanLow; }
        else if (SeaLevelAirpressure > 950 && SeaLevelAirpressure <= 980) { currentPressureCategory = PressureRanges.StormLow; }
        else if (SeaLevelAirpressure > 980 && SeaLevelAirpressure <= 1006) { currentPressureCategory = PressureRanges.Low; }
        else if (SeaLevelAirpressure > 1006 && SeaLevelAirpressure <= 1020) { currentPressureCategory = PressureRanges.Neutral; }
        else if (SeaLevelAirpressure > 1020 && SeaLevelAirpressure <= 1035) { currentPressureCategory = PressureRanges.High; }
        else if (SeaLevelAirpressure > 1035 && SeaLevelAirpressure <= 1084) { currentPressureCategory = PressureRanges.VeryHigh; }
    }
    private void analyseRelativeHumidity() //Sets the currentHumidityRange enumeration based on the relative humidity.
    {
        if (SeaLevelHumidity > 0 && SeaLevelHumidity <= 15) { currentHumidityCategory = HumidityRanges.ExtremelyDry; }
        else if (SeaLevelHumidity > 15 && SeaLevelHumidity <= 25) { currentHumidityCategory = HumidityRanges.Arid; }
        else if (SeaLevelHumidity > 25 && SeaLevelHumidity <= 55) { currentHumidityCategory = HumidityRanges.Moderate; }
        else if (SeaLevelHumidity > 55 && SeaLevelHumidity <= 75) { currentHumidityCategory = HumidityRanges.Humid; }
        else if (SeaLevelHumidity > 75) { currentHumidityCategory = HumidityRanges.Oppressive; }
    }

    //Conditional Case Switchers 
        private void runHumidityConditionals()              //Precipitation - Yes/No
    {
        analyseRelativeHumidity();
        switch (currentHumidityCategory)
        {
            case HumidityRanges.ExtremelyDry:
                PrecipitationPossible = false;
                break;
            case HumidityRanges.Arid:
                PrecipitationPossible = false;
                break;
            case HumidityRanges.Moderate:
                PrecipitationPossible = true;
                break;
            case HumidityRanges.Humid:
                PrecipitationPossible = true;
                break;
            case HumidityRanges.Oppressive:
                PrecipitationPossible = true;
                break;
        }
    }    
        private void runPressureConditionals()              //Clouds & Wind - Yes/No & Strength
    {
        analyseAirPressure(); //This will return the PressureRanges case.

        switch (currentPressureCategory)
        {
            case PressureRanges.HuracanLow:
                CloudFormationPossible = true;
                    
                    currentWindType = WindTypes.Violent;
                break;

            case PressureRanges.StormLow:
                CloudFormationPossible = true;
                    
                    currentWindType = WindTypes.Storm;
                break;

            case PressureRanges.Low:
                CloudFormationPossible = true;
                    
                    currentWindType = WindTypes.FluctuatingStrong;
                break;

            case PressureRanges.Neutral:
                CloudFormationPossible = true;
                    
                    currentWindType = WindTypes.Moderate;
                break;

            case PressureRanges.High:
                    
                if (SeaLevelAirpressure < 1025) { CloudFormationPossible = true;  }
                else { CloudFormationPossible = false; CloudPercentageLevel = 0; }
                currentWindType = WindTypes.Calm;
                break;

            case PressureRanges.VeryHigh:
                CloudFormationPossible = false; CloudPercentageLevel = 0;
                    currentWindType = WindTypes.Calm;
                break;
        }
    }
        private void runTemperatureConditionals()           //Precipitation - Type
        {
            if(CloudFormationPossible)
            {
                PrecipitationPossible = true;
            }
            else
            {
                PrecipitationPossible = false;
            }
            analyseAirTemperature();
            switch (currentTemperatureCategory)
            {
                case TemperatureRanges.ExtremelyCold: //-20°C to -40°C
                    if (CloudFormationPossible && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Snow;
                    }
                    else
                    {
                        currentPrecipitationType = PrecipitationTypes.None;
                    }
                    break;

                case TemperatureRanges.VeryCold: //-8°C to -20°C
                    if (CloudFormationPossible && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Snow;
                    }
                    else
                    {
                        currentPrecipitationType = PrecipitationTypes.None;
                    }
                    break;

                case TemperatureRanges.Frosty: //-8°C to 5°C
                    if (SeaLevelTemperature > 0 && CloudFormationPossible && PrecipitationPossible) { currentPrecipitationType = PrecipitationTypes.Sleet; }
                    else if (SeaLevelTemperature < 0 && CloudFormationPossible && PrecipitationPossible) { currentPrecipitationType = PrecipitationTypes.Snow; }
                    else { currentPrecipitationType = PrecipitationTypes.None; }
                    break;

                case TemperatureRanges.Moderate: //5°C to 24°C                     
                    if (SeaLevelTemperature > 5 && SeaLevelTemperature < 15 && SeaLevelHumidity > 70 && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Drizzle;
                    }
                    else if (SeaLevelTemperature > 20 && SeaLevelHumidity > 65 && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Hail;
                    }
                    else if (PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Rain;
                    }
                    else { currentPrecipitationType = PrecipitationTypes.None; }
                    break;

                case TemperatureRanges.Warm: //24°C to 32°C
                    if (SeaLevelTemperature > 5 && SeaLevelTemperature < 15 && SeaLevelHumidity > 70 && CloudFormationPossible && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Drizzle;
                    }
                    else if (SeaLevelTemperature > 20 && SeaLevelHumidity > 65 && CloudFormationPossible && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Hail;
                    }
                    else if (CloudFormationPossible && PrecipitationPossible)
                    {
                        currentPrecipitationType = PrecipitationTypes.Rain;
                    }
                    else { currentPrecipitationType = PrecipitationTypes.None; }
                    break;

                case TemperatureRanges.VeryHot: //32°C to 48°C
                    currentPrecipitationType = PrecipitationTypes.None;
                    break;

                case TemperatureRanges.HazardousHot: //48°C to 60°C
                    currentPrecipitationType = PrecipitationTypes.None;
                    break;
            }
        }

        //Intensity of Wind, Clouds, Fog & Precipitation 
        private void calculateGlobalCloudLevel()
        {
            //CloudPercentageLevel takes Tempeature, Humidity and Airpressure into consideration in this arbitrary formula.
            CloudPercentageLevel = Mathf.Clamp01((((SeaLevelHumidity / 100) * (SeaLevelAirpressure - MinimumAirpressure) * 0.1f) + ((SeaLevelHumidity / 100) * (SeaLevelTemperature - MinimumTemperature) * 2f)) / 100);           
            //print(CloudPercentageLevel);
        }
        private void calculateGlobalFogLevel()
    {//Implement Fog Falloff... somewhere? AircraftController???
        if (CloudFormationPossible)
        {
            #region Explanation
            /*
            *   Because Fog is basically just a cloud - it is indeed literally the same formula.
            *   Higher pressure levels at sea level prevent the fog / cloud moving up. However, it needs to be humid and cold enough for this to happen.
            *   The temperature inversion phenomenon keeps the fog on the ground, while there are also clouds at high altitude.
            *   Since the visibility of fog falls off exponentially, only really high levels (75&+) of fog will actually affect visibility. (and fog decreases over altitude, too.)
            */
            #endregion
            FogPercentageLevel = Mathf.Clamp01((((SeaLevelHumidity / 100) * (SeaLevelAirpressure - MinimumAirpressure) * 0.1f) + ((SeaLevelHumidity / 100) * (SeaLevelTemperature - MinimumTemperature) * 2f)) / 100);
        }
        else
        {
            FogPercentageLevel = 0;
        }
    }
        private void calculateGlobalPrecipitationIntensity()
    {       
        if (PrecipitationPossible)
        {
            PrecipitationIntensity = (int)Mathf.Round((CloudPercentageLevel / 100) * (SeaLevelHumidity / 100) * (SeaLevelAirpressure - MinimumAirpressure) * 0.05f);
            Mathf.Clamp(PrecipitationIntensity, 0, 100);
        }
        else
        {
            PrecipitationIntensity = 0;
        }
    }
        private void calculateGlobalWindIntensity()
    {
        WindIntensity = (100 - ((SeaLevelAirpressure - MinimumAirpressure) * 100) / (MaximumAirpressure - MinimumAirpressure)) / 100;
        Mathf.Clamp01(WindIntensity);
    }

        private void SwitchGlobalPrecipitation()
    {
            if (PrecipitationPossible)
            {
                switch (currentPrecipitationType)
                {
                    case PrecipitationTypes.Drizzle:

                        break;
                    case PrecipitationTypes.Hail:

                        break;
                    case PrecipitationTypes.Rain:

                        break;
                    case PrecipitationTypes.Sleet:

                        break;
                    case PrecipitationTypes.Snow:

                        break;
                    case PrecipitationTypes.None:
                        PrecipitationIntensity = 0;
                        break;
                }
            }
            else
            {
                currentPrecipitationType = PrecipitationTypes.None;
                switch (currentPrecipitationType)
                {
                    case PrecipitationTypes.None:
                        PrecipitationIntensity = 0;
                        break;
                }
            }
    }
    #endregion

}
}
