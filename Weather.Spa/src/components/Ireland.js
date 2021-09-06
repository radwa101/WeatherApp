import React, { useState } from "react";
import ReactDOM from "react-dom";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import PropTypes from 'prop-types';
import WeatherActions from '../actions/weather_actions'
import { Toolbar, Data } from "react-data-grid-addons";
import Loader from './Loader';
import TemperatureGrid from './TemperatureGrid'

function mapStateToProps(state) {
    return  {
            gridIsLoading: state.weatherReducer.toJS().gridIsLoading,
            locations: state.weatherReducer.toJS().irelandLocations
        };
}

function mapDispatchToPropsReposPage(dispatch) {
    return {
        actions: {
              weatherActions : bindActionCreators(WeatherActions, dispatch),
           }
        };
}

const RowTemperatureRenderer = ({ row, idx }) => {
    return (
            <div><span>{row.temperatureDisplay} </span></div>
    );
};

function getColumns(actions) { 
  var newColumns = [];
  newColumns.push({key: "city", name: "City", width: 150, filterable: true});
  newColumns.push({key: "weatherDescription", name: "Weather", width: 100, filterable: true});
  newColumns.push({key: "temperatureDisplay", name: "Temperature", formatter: RowTemperatureRenderer, width: 100, filterable: true});
  newColumns.push({key: "windSpeed", name: "Wind Speed", width: 100, filterable: true});
  newColumns.push({key: "humidity", name: "Humidity", width: 100, filterable: true});
  newColumns.push({key: "pressure", name: "Pressure", width: 100, filterable: true});
  newColumns.push({key: "dateTime", name: "Time", width: 100, filterable: true});
  return newColumns;
}

class Ireland extends React.Component {

  componentWillMount()
  {
      this.props.actions.weatherActions.getIrelandTemperatures();
  }

  render() {
      if(this.props.gridIsLoading)
      {
         return <Loader/>
      }

       var rows = this.props.locations;
       
       return (
           <TemperatureGrid rows={rows} columns={getColumns(this.props.actions)} />
        )
  }
}
export default connect(mapStateToProps, mapDispatchToPropsReposPage)(Ireland);
