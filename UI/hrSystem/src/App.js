import React, {useState, useEffect} from "react";
import { variables } from "./Variables.js";
import {Candidates} from "./Candidates"
import {BrowserRouter as Router, Route, Routes, NavLink} from 'react-router-dom';
require("es6-promise").polyfill();
require("isomorphic-fetch");

export default function SearchFunction() {
    const [data, setData] = useState([])
    const [queryName, setQueryName] = useState("")
    const [querySkill, setQuerySkill] = useState("")

   

    useEffect(() => {
        fetch(variables.API_URL + 'Candidates/')
        .then(response => response.json())
        .then(json => setData(json));
    }, []);

    function search(rows) {
      var skillQuery = querySkill.toLowerCase();
      var skillName = queryName.toLowerCase();
      return ((rows.filter(row => (row.candidate_name.toLowerCase().indexOf(skillName) > -1))).filter(row => row.skills.toLowerCase().indexOf(skillQuery) > -1));
    }
   
    return (
        <div>
          <div>
             <h1 className="d-flex justify-content-center m-3">
               CRUD
            </h1>    
          </div>
            <div>
              <Candidates data = {data}/>
            </div>
        </div>
        
    );
}
