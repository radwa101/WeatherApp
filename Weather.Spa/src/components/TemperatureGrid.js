import React, { useState } from "react";
import ReactDOM from "react-dom";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import PropTypes from 'prop-types';
import WeatherActions from '../actions/weather_actions'
import ReactDataGrid from "react-data-grid";
import { Toolbar, Data } from "react-data-grid-addons";
import Loader from './Loader';
import ForecastModal from './ForecastModal'
import { openModal } from './ForecastModal'

const selectors = Data.Selectors;

function mapStateToProps(state) {
    return  {
            locations: state.weatherReducer.toJS().worldLocations,
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

function handleFilterChange(filter) {
    const newFilters = {};
    if(filter.filterTerm !== undefined) {
        if (filter.filterTerm) {
            newFilters[filter.column.key] = filter;
        } else {
            delete newFilters[filter.column.key];
        }
    }
    return newFilters;
};


function getRows(rows, filters) {
  return selectors.getRows({ rows, filters });
}

class TemperatureGrid extends React.Component {
    render() {
        const columns = this.props.columns.length === 0 ? getColumns(this.props.actions) : this.props.columns;
        const filteredRows = getRows(this.props.rows, this.props.filters);
        return (
                <div>
                    <ReactDataGrid
                        columns={columns}
                        rowGetter={i => filteredRows[i]}
                        rowsCount={getRows(this.props.rows, this.props.filters).length}
                        minHeight={450}
                        toolbar={<Toolbar enableFilter={true} />}
                        onAddFilter={filter => this.props.actions.weatherActions.updateFilters(handleFilterChange(filter))}
                        onClearFilters={() => this.props.actions.weatherActions.updateFilters({})}
                    />
                    <ForecastModal />
                </div>
        );
    }
}
export default connect(mapStateToProps, mapDispatchToPropsReposPage)(TemperatureGrid);