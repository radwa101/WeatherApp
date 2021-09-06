import * as fetch from "isomorphic-fetch";
import * as Immutable from "immutable";
import { WeatherService } from "../services/weather_service";

var weatherService = new WeatherService();

let getWorldTemperatures = () => {
    return (dispatch) => {
       dispatch({ type: 'FETCH_WORLDWEATHER_BEGIN' })
       let status = 0;
       weatherService.getWorldWeather()
       .then(function(response){
                    status = response.status;
                    return response.json();
            }).then((responseJSON) => {
                    if (status === 200){
                        dispatch({ 
                            type: 'FETCH_WORLDWEATHER_SUCCESS',
                            data : Immutable.fromJS(responseJSON)
                        })
                    }
            });      
    }
}

let getEuropeanTemperatures = () => {
    return (dispatch) => {
       dispatch({ type: 'FETCH_EUROPEANWEATHER_BEGIN' })
       let status = 0;
        weatherService.getEuropeanWeather()
       .then(function(response){
                    status = response.status;
                    return response.json();
            }).then((responseJSON) => {
                    if (status === 200){
                        dispatch({ 
                            type: 'FETCH_EUROPEANWEATHER_SUCCESS',
                            data : Immutable.fromJS(responseJSON)
                        })
                    }
            });
    }
}

let getIrelandTemperatures = () => {
    return (dispatch) => {
       dispatch({ type: 'FETCH_IRELANDWEATHER_BEGIN' })
       let status = 0;
        weatherService.getIrelandWeather()
       .then(function(response){
                    status = response.status;
                    return response.json();
            }).then((responseJSON) => {
                    if (status === 200){
                        dispatch({ 
                            type: 'FETCH_IRELANDWEATHER_SUCCESS',
                            data : Immutable.fromJS(responseJSON)
                        })
                    }
            });
    }
}

let getExtremeTemperatures = (filter, selectedRegionOption) => {
    return (dispatch) => {
       dispatch({ type: 'FETCH_EXTREMEWEATHER_BEGIN' })
       let status = 0;
        weatherService.getExtremeWeather(filter)
       .then(function(response){
                    status = response.status;
                    return response.json();
            }).then((responseJSON) => {
                    if (status === 200){
                        dispatch({ 
                            type: 'FETCH_EXTREMEWEATHER_SUCCESS',
                            data : Immutable.fromJS(responseJSON),
                            selectedOption: Immutable.fromJS(selectedRegionOption)
                        })
                    }
            });
    }
}

let getForecastData = (filter) => {
    return (dispatch) => {
       dispatch({ 
           type: 'FETCH_FORECASTDATA_BEGIN',
           data: '<div id=\"container\"><div>Loading ...</div></div>'
        })
       let status = 0;
        weatherService.getForecastData(filter)
       .then(function(response){
                    status = response.status;
                    return response.json();
            }).then((responseJSON) => {
                    if (status === 200){
                        dispatch({ 
                            type: 'FETCH_FORECASTDATA_SUCCESS',
                            data : Immutable.fromJS(responseJSON),
                        })
                    }
            });
    }
}

let showForecastDataModal = (showModal, title) => {
    return (dispatch) => {
       dispatch({ 
            type: 'SHOW_FORECASTDATA_SUCCESS',
            isModalVisible : showModal,
            forecastTitle: title
        })
    }
}

let updateFilters = (filters) => {
    return (dispatch) => {
       dispatch({ 
            type: 'UPDATE_FILTERS_SUCCESS',
            filters : filters
        })
    }
}


let addCity = (city) => {
    return (dispatch) => {
      dispatch({ 
         type: 'ADD_CITY',
         data: city
     })
    }
}


let WeatherActions = {
   getWorldTemperatures,
   getEuropeanTemperatures,
   getIrelandTemperatures,
   getExtremeTemperatures,
   getForecastData,
   showForecastDataModal,
   addCity,
   updateFilters
};

export default WeatherActions;

