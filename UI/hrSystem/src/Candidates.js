import React, {Component} from 'react';
import { Skills } from './Skills.js';
import { variables } from "./Variables.js";
require("es6-promise").polyfill();
require("isomorphic-fetch");






export class Candidates extends Component{

    constructor(props){
        super(props);

        this.state={
            candidates:[],
            modalTitle: "",
            candidate_id: 0, 
            searchByName: "",
            searchBySkill: ""         
        }
    }

    setQueryName(name)
    {
        this.state.searchByName = name
        this.searchClick();
    }

    setQuerySkill(skill)
    {
        this.state.searchBySkill = skill
        this.searchClick();
    }

    refreshList(partName = "", skill = ""){
        var uri;

        if(partName != "" && skill !="")
        {
            uri = '?partOfName=' + partName + '&skillName=' + encodeURIComponent(skill);
        }
        else if(partName == "" && skill != "")
        {
            uri = '?skillName=' + encodeURIComponent(skill);
        }
        else if (partName != "" && skill == "")
        {
            uri = '?partOfName=' + partName;
        }
        else
        {
            uri = '';
        }
        fetch(variables.API_URL+'Candidates' + uri)
        .then(response=>response.json())
        .then(data=>{
            this.setState({candidates:data});
        });
    }

    componentDidMount(){
        this.refreshList();
    }

    changeCandidateName = (e) => {
        this.setState({candidate_name: e.target.value})
    }

    changeDateOfBirth = (e) => {
        this.setState({date_of_birth: e.target.value})
    }

    changeContactNumber =(e)=>{
        this.setState({contact_number:e.target.value});
    }

    changeEmail =(e)=>{
        this.setState({email:e.target.value});
    }    

    changeCandidateSkills = (e) => {
        this.setState({skills: e.target.value})
    }

    addClick(){
        this.setState({
            modalTitle: "Add Candidate",
            candidate_id: 0,
        });
    }

    

    editClick(can){
        this.setState({
            modalTitle: "Edit Candidate",
            candidate_id: can.candidate_id,
            candidate_name: can.candidate_name,
            date_of_birth: can.date_of_birth,
            contact_number: can.contact_number,
            email: can.email,
            skills: can.skills
        });
    }
    

    createClick() {
        console.log(variables.API_URL + 'Candidates?candidateName='+this.state.candidate_name+'&date='+this.state.date_of_birth+'&contactNumber='+this.state.contact_number+'&email='+this.state.email+'&skills='+ this.state.skills)

        fetch(variables.API_URL + 'Candidates/'+ encodeURIComponent(this.state.skills), {
            method: 'POST',
            headers: {
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                name: this.state.candidate_name,
                dateOfBirth: this.state.date_of_birth,
                contactNumber: this.state.contact_number,
                email: this.state.email
            })
        })
        .then(response => response.json())
        .then((result)=> {
            alert(result);
            this.refreshList();
        }, (error) => {
            console.log(error)
            alert('Failed');
        })
    }

    updateClick(id) {
        console.log(id + this.state.date_of_birth + this.state.contact_number + this.state.email)
        fetch(variables.API_URL + 'Candidates/' + encodeURIComponent(this.state.skills), {
            method: 'PUT',
            headers: {
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                id: id,
                name: this.state.candidate_name,
                dateOfBirth: this.state.date_of_birth,
                contactNumber: this.state.contact_number,
                email: this.state.email
            })
        })
        .then(response => response.json())
        .then((result)=> {
            alert(result);
            this.refreshList();
        }, (error) => {
            console.log(error)
            alert('Failed');
        })
    }

    deleteClick(id) {
        if(window.confirm('Are you sure?')){
        fetch(variables.API_URL + 'Candidates?id=' + id, {
            method: 'DELETE',
            headers: {
                'Accept':'application/json',
                'Content-Type':'application/json'
            }
        })
        .then(response => response.json())
        .then((result)=> {
            alert(result);
            this.refreshList();
        }, (error) => {
            console.log(error)
            alert('Failed');
        })
        }
    }

    searchClick()
    {
        this.refreshList(this.state.searchByName, this.state.searchBySkill);
    }

    
    // searchItems = () => {
        
    // }

    render(){
        const {
            candidates,
            modalTitle,
            candidate_name,
            date_of_birth,
            contact_number,
            email,
            candidate_id,
            skills
        }=this.state;

        return(
            <div>
                
                    <input type="text" placeholder="Search by name" id="sbn" value={this.searchByName} onChange = {(e) => this.setQueryName(e.target.value)}/>
                    <input type="text" placeholder="Search by skill" id="sbs" value={this.searchBySkill} onChange = {(e) => this.setQuerySkill(e.target.value)}/>
               
                

                <button type="button"
                className=" btn btn-primary m-2 float-end"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                onClick={()=>this.addClick()}>
                    Add Candidate
                </button>
                <table className="table table-striped">
                <thead>
                <tr>
                    <th>
                       ID
                    </th>
                    <th>
                        Name
                    </th>
                    <th>
                        Date of birth
                    </th>
                    <th>
                        Contact number
                    </th>
                    <th>
                        Email
                    </th>
                    <th>
                        Skills
                    </th>
                    <th>
                        Options
                    </th>
                </tr>
                </thead>
                <tbody>
                    {candidates.map(can=>
                        <tr key={can.candidate_id}>
                            <td>{can.candidate_id}</td>
                            <td>{can.candidate_name}</td>
                            <td>{can.date_of_birth}</td>
                            <td>{can.contact_number}</td>
                            <td>{can.email}</td>
                            <td>{can.skills}</td>
                            <td>

                            
                            <button type="button"
                            className="btn btn-light mr-1"
                                data-bs-toggle="modal"
                                data-bs-target="#exampleModal"
                                onClick={()=>this.editClick(can)}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
                                </svg>
                            </button>

                            <button type="button"
                            className="btn btn-light mr-1"
                            onClick={()=>this.deleteClick(can.candidate_id)}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                                <path fillRule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                                </svg>
                            </button>
                            </td>
                        </tr>
                        )}
                </tbody>
                </table>

                <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{modalTitle}</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"
                        ></button>
                    </div>

                    <div className="modal-body">
                        <div className="input-group mb-3">
                            <span className="input-group-text">Candidate name</span>
                            {
                                candidate_id == 0 ?
                                <input type="text" className="form-control"
                                value={candidate_name}
                                onChange = {this.changeCandidateName}/>
                                : <input type="text" disabled = "disabled"  className="form-control"
                                value={candidate_name}/>
                               
                            }
                            
                        </div>
                    </div> 

                    <div className="modal-body">
                        <div className="input-group mb-3">
                            <span className="input-group-text">Date of birth</span>
                            {
                                candidate_id == 0 ?
                                    <input type="text" className="form-control"
                                    value={date_of_birth}
                                    onChange = {this.changeDateOfBirth}/> 
                                    : <input type="text" disabled = "disabled" className="form-control"
                                    value={date_of_birth}/>
                            }
                            
                        </div>
                    </div>

                    <div className="modal-body">
                        <div className="input-group mb-3">
                            <span className="input-group-text">contact_number</span>
                            {
                                candidate_id == 0 ?
                                <input type="text" className="form-control"
                                value={contact_number}
                                onChange = {this.changeContactNumber}/>
                                : <input type="text" disabled = "disabled" className="form-control"
                                value={contact_number}/>
                            }
                            
                        </div>
                    </div>

                    <div className="modal-body">
                        <div className="input-group mb-3">
                            <span className="input-group-text">Email</span>
                            {
                                candidate_id == 0 ?
                                <input type="text" className="form-control"
                                value={email}
                                onChange = {this.changeEmail}/> 
                                : <input type="text" disabled = "disabled" className="form-control"
                                value={email}/> 
                            }
                                                    
                        </div>
                    </div>       

                     <div className="modal-body">
                        <div className="input-group mb-3">
                            <span className="input-group-text">Skills</span>
                            <input type="text" className="form-control"
                            value={skills}
                            onChange = {this.changeCandidateSkills}/> 
                        </div>
                    </div>                 

                    {candidate_id==0?
                    <button type="button"
                    className="btn btn-primary float-start"
                    onClick={()=>this.createClick()}
                    >Create</button>
                    :null}

                    {candidate_id!=0?
                    <button type="button"
                    className="btn btn-primary float-start"
                    onClick={()=>this.updateClick(candidate_id)}
                    >Update</button>
                    :null}                
                </div>
                </div>
                </div>       

                
            </div>
        )
    }
}