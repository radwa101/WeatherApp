import { combineReducers } from 'redux'
import weatherReducer from './weatherReducer'
//import usersReducer from './usersReducer'

const rootReducer = combineReducers({
  weatherReducer
  //,usersReducer
})

export default rootReducer
