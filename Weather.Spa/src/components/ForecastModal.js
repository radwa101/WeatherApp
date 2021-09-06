import React, { useState } from "react";
import ReactDOM from "react-dom";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import PropTypes from 'prop-types';
import WeatherActions from '../actions/weather_actions'
import ReactDataGrid from "react-data-grid";
import { Toolbar, Data } from "react-data-grid-addons";
import Loader from './Loader';
import Modal from 'react-awesome-modal';

function mapStateToProps(state) {
    return  {
            modalIsLoading: state.weatherReducer.toJS().modalIsLoading,
            locations: state.weatherReducer.toJS().worldLocations,
            isModalVisible: state.weatherReducer.toJS().isModalVisible,
            filters: state.weatherReducer.toJS().filters,
            forecastTable: state.weatherReducer.toJS().forecastTable,
            title: state.weatherReducer.toJS().forecastTitle
        };
}

function mapDispatchToPropsReposPage(dispatch) {
    return {
        actions: {
              weatherActions : bindActionCreators(WeatherActions, dispatch),
           }
        };
}

export function openModal(title, location, filterTable, actions) {
    actions.weatherActions.showForecastDataModal(true, title);
    actions.weatherActions.getForecastData("filter=" + location.ForecastDetailsUrl + "&filterTable=" + filterTable);
}

function closeModal(actions, filters) {
    actions.weatherActions.showForecastDataModal(false, '');
}

const RenderModalContent = ({props}) => {
    return <div>
        <span style={{fontSize: 20}}>{props.title}</span>
        <br/>
        <div style={{fontSize: 10}} dangerouslySetInnerHTML={
                { __html: props.forecastTable }} />
        <br/>
        <a href="javascript:void(0);" onClick={() => closeModal(props.actions, props.filters)}>Close</a>
    </div>
}; 

class ForecastModal extends React.Component {
    render() {
        return (
                <div>
                    <Modal 
                        visible={this.props.isModalVisible}
                        width="636"
                        height="450"
                        effect="fadeInUp"
                        onClickAway={() => closeModal(this.props.actions, this.props)}
                        >
                        <RenderModalContent props={this.props} />    
                    </Modal>
                </div>
        );
    }
}
export default connect(mapStateToProps, mapDispatchToPropsReposPage)(ForecastModal);