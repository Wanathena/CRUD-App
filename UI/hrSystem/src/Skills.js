import React, {Component} from 'react';
import { findAllInRenderedTree } from 'react-dom/test-utils';
import { Candidates } from './Candidates.js';
import { variables } from './Variables.js';

export class Skills extends Component {
    constructor(props) {
        super(props);

        this.state = {
            skills:[],
            modalTitle: "",
            skill_name: "",
            skill_id: 0
        }
    }

    refreshList() {
        fetch(variables.API_URL + 'Skills')
        .then(response => response.json())
        .then(data => {
            this.setState({skills: data});
        })
    }

    componentDidMount() {
        this.refreshList();
    }

    addClick() {
        this.setState({
            modalTitle: "Add Skill",
            skill_id: 0
        });
    }

    createClick() {
        fetch(variables.API_URL + 'Skills?skillName=' + this.state.skill_name, {
            method: 'POST',
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

    deleteClick(id) {
        if(window.confirm('Are you sure?')){
        fetch(variables.API_URL + 'Skills?id=' + id, {
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

    
    changeSkillName = (e) => {
        console.log(e.target.value)
        this.setState({skill_name: e.target.value})
        console.log(this.state.skill_name)
    }

    render() {
        const {
            skills,
            modalTitle,
            skill_name,
            skill_id
        } = this.state;

        return(
            <div>
                <button type = "button"
                className = "btn btn-primary m-2 float-end"
                data-bs-toggle = "modal"
                data-bs-target = "#exampleModal"

                onClick={() => this.addClick()}>
                    Add Skill
                </button>

                <table className = "table table-striped">
                    <thead>
                        <tr>
                            <th>
                                Skill ID
                            </th>
                            <th>
                                Skill Name
                            </th>
                            <th>
                                Options
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            skills.map(skill => 
                                <tr key = {skill.skill_id}>
                                    <td>{skill.skill_id}</td>
                                    <td>{skill.skill_name}</td>

                                    <td>
                                        <button type = "button"
                                        className = "btn btn-light mr-1"
                                        onClick = {() => this.deleteClick(skill.skill_id)}>
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                                            <path fillRule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                                            </svg>
                                        </button>
                                    </td>
                                </tr>
                                )
                        }
                    </tbody>
                </table>

                <div className = "modal fade" id = "exampleModal" tabIndex = "-1" aria-hiddent = "true">
                <div className = "modal-dialog modal-lg modal-dialog-centered">
                <div className = "modal-content">
                    <div className = "modal-header">
                        <h5 className = "modal-title">{modalTitle}</h5>
                        <button type = "button" className = "btn-close" data-bs-dismiss = "modal" aria-label = "Close"></button>
                    </div>
                    <div className = "modal-body">
                        <div className = "input-group mb-3">
                            <span className = "input-group-text"> Skill name </span>
                            <input type = "text" className = "form-control"
                            value = {skill_name}
                            onChange = { this.changeSkillName}/>
                        </div>
                    </div>

                    {skill_id == 0 ?
                        <button type = "button"
                        className = "btn btn-primary float-start"
                        onClick = {() => this.createClick()}>
                        Create
                        </button>
                    : null }
                </div>
                </div>
                </div>

            </div>
        )
    }
}