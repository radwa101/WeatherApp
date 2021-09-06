import React from 'react'
import { render } from 'react-dom'
import { createStore, compose, applyMiddleware } from 'redux'
import { Provider } from 'react-redux'
import rootReducer from './reducers'
import Cities from './components/Cities'
import World from './components/World'
import thunk from 'redux-thunk';
import { Router, Route, IndexRoute } from "react-router";
import { syncHistoryWithStore } from "react-router-redux";
import Main from "./components/Main";
import "./css/index.css"

const store = createStore(rootReducer
    ,compose(applyMiddleware(thunk)
    ,window.devToolsExtension ? window.devToolsExtension() : f => f)
);

render(
    <Provider store={ store }>
        <Main />
    </Provider>,
  document.getElementById('root')
)
