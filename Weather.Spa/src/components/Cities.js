import * as React from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import PropTypes from 'prop-types';
import WeatherActions from '../actions/weather_actions'
import Loader from './Loader'

function mapStateToProps(state) {
    return  {
              weatherRepo: state.weatherReducer.toJS()
            };
}

function mapDispatchToPropsReposPage(dispatch) {
    return {
        actions: {
              actionsWeather : bindActionCreators(WeatherActions, dispatch),
           }
        };
}


class Cities extends React.Component {

  componentWillMount()
  {
      this.props.actions.actionsWeather.getCities();
  }

  
  render() {
      
      if(this.props.weatherRepo.gridIsLoading)
      {
         return <Loader/>
      }

      let cityItems = this.props.weatherRepo.cities.map(function(city, i)
           { return <li key={i}>{city.id} - {city.label}</li>
           })


      let newCity = { id: "104", label: "Four" }

      return (
        <div>
          <ul>{cityItems}</ul>
          <button onClick={() => this.props.actions.actionsWeather.addCity(newCity)}>Click me</button>
        </div>
      );
  } 
}
export default connect(mapStateToProps, mapDispatchToPropsReposPage)(Cities);

