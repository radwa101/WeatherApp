import React, { useState } from "react";
import ReactDOM from "react-dom";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import PropTypes from 'prop-types';
import WeatherActions from '../actions/weather_actions'
import { Toolbar, Data } from "react-data-grid-addons";
import Loader from './Loader';
import Select from 'react-select';
import "../css/index.css"
import TemperatureGrid from './TemperatureGrid'

const selectors = Data.Selectors;

function mapStateToProps(state) {
    return  {
            gridIsLoading: state.weatherReducer.toJS().gridIsLoading,
            locations: state.weatherReducer.toJS().extremeLocations,
            selectedRegionOption: state.weatherReducer.toJS().selectedRegionOption
        };
}

function mapDispatchToPropsReposPage(dispatch) {
    return {
        actions: {
              weatherActions : bindActionCreators(WeatherActions, dispatch),
           }
        };
}

function getColumns(isTemp) { 
  var newColumns = [];
  newColumns.push({key: "city", name: "City", width: 350, filterable: true});
  newColumns.push({key: "temperatureDisplay", name: isTemp ? "Temperature" : "Rainfall Amount", width: isTemp ? 100 : 150, filterable: true});
  return newColumns;
}

class Extremes extends React.Component {

  componentWillMount()
  {
      var filter = "region=" + this.props.selectedRegionOption.value + "&filter=" + this.props.filter + "&isTemp=" + this.props.isTemp;
      this.props.actions.weatherActions.getExtremeTemperatures(filter, this.props.selectedRegionOption);
  }

  handleChange(selectedOption) {
      var filter = "region=" + selectedOption.value + "&filter=" + this.props.filter + "&isTemp=" + this.props.isTemp;
      this.props.actions.weatherActions.getExtremeTemperatures(filter, selectedOption);
  }
  
  render() {
     const options = [
       { value: 'World', label: 'World' },
       { value: 'Irel', label: 'Ireland' },
       { value: 'North', label: 'Northern Hemisphere' },
       { value: 'South', label: 'Southern Hemisphere' },
       { value: 'Cana', label: 'Canada' },
       { value: 'United%2BS', label: 'United States' },
       { value: 'Eur', label: 'Europe' },
       { value: 'United%2BK', label: 'United Kingdom' }
     ];

      if(this.props.gridIsLoading)
      {
         return <Loader/>
      }

       var rows = this.props.locations;
       
       return (
           <div>
              <h3 style={{ marginTop: "0"}}>{this.props.title}</h3>
              <Select 
                    value={this.props.selectedRegionOption} 
                    onChange={this.handleChange.bind(this)}
                    options={options} />
              <TemperatureGrid rows={rows} columns={getColumns(this.props.isTemp)} isTemp={this.props.isTemp} />
           </div>
        )
  }

}


export default connect(mapStateToProps, mapDispatchToPropsReposPage)(Extremes);
