import React, { Component } from "react";
import { Route, NavLink, HashRouter } from "react-router-dom";
import Header from "./Header";

class Main extends Component {
  render() {
    return (
        <HashRouter>
            <Header />
        </HashRouter>
    );
  }
}
 
export default Main;