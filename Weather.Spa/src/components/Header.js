import React, {Component} from 'react';
import { Route, NavLink, HashRouter } from "react-router-dom";
import World from "./World";
import Europe from "./Europe";
import Ireland from "./Ireland";
import Extremes from "./Extremes";

class Header extends Component {
    render(){
        return(
            <div>
                <ul className="weatherMenu">
                    <li><a>Popular Lists:</a></li>
                    <li><NavLink exact to="/">World</NavLink ></li>
                    <li><NavLink  to="/Europe">Europe</NavLink ></li>
                    <li><NavLink  to="/Ireland">Ireland</NavLink ></li>
                    <li><NavLink  to="/Extremes/Highest">Highest Temperatures</NavLink ></li>
                    <li><NavLink  to="/Extremes/Lowest">Lowest Temperatures</NavLink ></li>
                    <li><NavLink  to="/Extremes/Rainfall">Rainfall</NavLink ></li>
                </ul>
                <div className="weatherContent">
                    <Route exact path="/" component={World}/>
                    <Route path="/Europe" component={Europe}/>
                    <Route path="/Ireland" component={Ireland}/>
                    <Route path="/Extremes/Highest" render={()=><Extremes filter="&quot;margin-top:5px;%20width:%20800px;&quot;" isTemp={true} title="Maximum Temperature Last 24h" />} />
                    <Route path="/Extremes/Lowest" render={()=><Extremes filter="%22width:%2099%;%22" isTemp={true} title="Lowest Temperature Last 24h" />} />
                    <Route path="/Extremes/Rainfall" render={()=><Extremes filter="%22width:%2099%;%22" isTemp={false} title="Maximum Rainfall Last 24h" />} />
                </div>
            </div>
        );
    }
}
export default Header