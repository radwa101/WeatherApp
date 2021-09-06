import React, { useState } from "react";
import ReactDOM from "react-dom";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import PropTypes from 'prop-types';
import WeatherActions from '../actions/weather_actions'
import { Toolbar, Data } from "react-data-grid-addons";
import Loader from './Loader';
import TemperatureGrid from './TemperatureGrid'
import ForecastModal from './ForecastModal'
import { openModal } from './ForecastModal'

function mapStateToProps(state) {
    return  {
            gridIsLoading: state.weatherReducer.toJS().gridIsLoading,
            locations: state.weatherReducer.toJS().europeanLocations,
            isModalVisible: state.weatherReducer.toJS().isModalVisible,
            filters: state.weatherReducer.toJS().filters,
            forecastTable: state.weatherReducer.toJS().forecastTable
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
    return <div><span>{row.TemperatureDisplay} </span> <img src={row.Icon} /></div>
};

const LocationRenderer = ({ row, actions }) => {
    return <div>
                <span href="javascript:void(0)"> 
                    <span style={{color: "#0000EE", marginRight: 20}} onClick={() => openModal("Forecast for the next 5 hours", row, "wt-5hr", actions)}><u> 5 hrs </u></span>
                    <span style={{color: "#0000EE", marginRight: 20}} onClick={() => openModal("Forecast for the next 48 hours", row, "wt-48", actions)}><u> 48 hrs </u></span>
                    <span style={{color: "#0000EE", marginRight: 20}} onClick={() => openModal("Forecast for the next 2 weeks", row, "wt-14d", actions)}><u> 2 wks </u></span>
                </span>
            </div>
};

function getColumns(actions) { 
  var newColumns = [];
  newColumns.push({key: "LocationDisplayed", name: "City", width: 300, filterable: true, });
  newColumns.push({key: "Forecast", name: "Forecast", formatter: <LocationRenderer actions={actions} />, width: 200, filterable: true, }); 
  newColumns.push({key: "TemperatureDisplay", name: "Temperature", formatter: RowTemperatureRenderer, width: 100, filterable: true, });
  newColumns.push({key: "DateTime", name: "Time", width: 100, filterable: true });      
  return newColumns;
}


class Europe extends React.Component {

  componentWillMount()
  {
      this.props.actions.weatherActions.getEuropeanTemperatures();
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

export default connect(mapStateToProps, mapDispatchToPropsReposPage)(Europe);
