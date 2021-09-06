import * as Immutable from "immutable";

const initialState = Immutable.fromJS({
                 worldLocations: [],
                 europeanLocations: [],
                 irelandLocations: [],
                 gridIsLoading: true,
                 modalIsLoading: true,
                 isSettingsSaving: false,
                 selectedRegionOption: { value: 'World', label: 'World' },
                 isModalVisible: false,
                 forecastTable: '',
                 filters: {},
                 forecastTitle: ''
                });
                

const weatherReducer = (previousState = initialState, action) => 
{
  switch (action.type) {
    case 'FETCH_WORLDWEATHER_BEGIN':
      return previousState.set('gridIsLoading', true);
    case 'FETCH_WORLDWEATHER_SUCCESS':
      previousState = previousState.set('gridIsLoading', false);
      previousState = previousState.set('worldLocations', action.data);
      previousState = previousState.set('forecastTable', '');
      return previousState;
    case 'FETCH_EUROPEANWEATHER_BEGIN':
      return previousState.set('gridIsLoading', true);
    case 'FETCH_EUROPEANWEATHER_SUCCESS':
      previousState = previousState.set('gridIsLoading', false);
      previousState = previousState.set('europeanLocations', action.data);
      return previousState;
    case 'FETCH_IRELANDWEATHER_BEGIN':
      return previousState.set('gridIsLoading', true);
    case 'FETCH_IRELANDWEATHER_SUCCESS':
      previousState = previousState.set('gridIsLoading', false);
      previousState = previousState.set('irelandLocations', action.data);
      return previousState;
    case 'FETCH_EXTREMEWEATHER_BEGIN':
      return previousState.set('gridIsLoading', true);
    case 'FETCH_EXTREMEWEATHER_SUCCESS':
      previousState = previousState.set('gridIsLoading', false);
      previousState = previousState.set('extremeLocations', action.data);
      previousState = previousState.set('selectedRegionOption', action.selectedOption);
      return previousState;
    case 'SHOW_FORECASTDATA_SUCCESS':
      previousState = previousState.set('isModalVisible', action.isModalVisible);
      previousState = previousState.set('gridIsLoading', false);
      previousState = previousState.set('forecastTitle', action.forecastTitle);
      return previousState;
    case 'FETCH_FORECASTDATA_BEGIN':
      previousState = previousState.set('forecastTable', action.data);
      return previousState.set('modalIsLoading', true);
    case 'FETCH_FORECASTDATA_SUCCESS':
      previousState = previousState.set('forecastTable', action.data);
      previousState = previousState.set('modalIsLoading', false);
      return previousState;
    case 'UPDATE_FILTERS_SUCCESS':
      previousState = previousState.set('filters', action.filters);
      return previousState;
    default:
      return previousState
  }
}

export default weatherReducer