export default class API {
}

const users = [];
const AUTH_KEY = 'currentUser';

export class SAID {
  constructor(ifless, ifequal) {
    this.channel = new BroadcastChannel('SAID');
    this.getSAID = () =>
      sessionStorage.SAID
        ? sessionStorage.SAID
        : (sessionStorage.SAID = Date.now());
    this.postSAID = () =>
      this.channel.postMessage({ SAID: this.getSAID() });
    this.close = () => this.channel.close();
    this.ifless = ifless;
    this.ifequal = ifequal;
    this.channel.onmessage = (event) => {
      if (event.data.SAID < this.getSAID()) this.ifless();
      else if (event.data.SAID === this.getSAID()) this.ifequal();
      else this.postSAID();
    };
  }
}

export class SA {
  static START = {
    A: 'A',
    N: 'N',
  };

  static COMMAND = {
    REG: 'REG',
    SIN: 'SIN',
    CAN: 'CAN',
    ADE: 'ADE',
    LIN: 'LIN',
    SOU: 'SOU',
    REJ: 'REJ',
    RSU: 'RSU',
  };

  static STATES = {
    A: SA.START.A,
    N: SA.START.N,
    S: 'S',
    R: 'R',
    SV: 'SV',
    RV: 'RV',
  };

  static TRANFUNC = {
    [SA.STATES.A]: { [SA.COMMAND.SOU]: SA.STATES.N },
    [SA.STATES.N]: { [SA.COMMAND.SIN]: SA.STATES.S, [SA.COMMAND.REG]: SA.STATES.R },
    [SA.STATES.S]: { [SA.COMMAND.SIN]: SA.STATES.SV, [SA.COMMAND.CAN]: SA.STATES.N },
    [SA.STATES.R]: { [SA.COMMAND.REG]: SA.STATES.RV, [SA.COMMAND.CAN]: SA.STATES.N },
    [SA.STATES.SV]: { [SA.COMMAND.LIN]: SA.STATES.A, [SA.COMMAND.ADE]: SA.STATES.S },
    [SA.STATES.RV]: { 
      [SA.COMMAND.LIN]: SA.STATES.A, 
      [SA.COMMAND.REJ]: SA.STATES.R,
      [SA.COMMAND.RSU]: SA.STATES.S 
    }
  };

  constructor(startState) {
    this.state = (SA.START[startState] == undefined) ? startState : SA.START.A;
    this.subscribers = {};

    this.on = (state, callback) => {
      if (this.subscribers[state] == undefined) {
        this.subscribers[state] = [];
      }
      this.subscribers[state].push(callback);
    };

    this.emit = (state, data) => {
      if (this.subscribers[state] != undefined) {
        this.subscribers[state].forEach(callback => callback(data));
      }
    };

    this.RunCommand = (s) => {
      const command = SA.TRANFUNC[this.state][s];
      if (command) {
        console.log(this.state, ':RunCommand: command =', command, ', s=', s);
        this.state = command;
        this.emit(this.state, SA.TRANFUNC[this.state]);
      }
      return { this: this.state };
    };
  }
}

export async function Registration(name, pass, sin, rej) {
  console.log("Registration attempt:", name);
  
  try {
    // Проверка на пустые имя или пароль
    if (!name.trim() || !pass.trim()) {
      console.log("Registration failed: empty username or password");
      rej();
      return false;
    }
    
    const userExists = users.some(user => user.name === name);
    if (!userExists) {
      users.push({ name, pass });
      console.log("Registration successful");
      
      sin();
      return true;
    } else {
      console.log("Registration failed: user exists");
      rej();
      return false;
    }
  } catch (error) {
    console.log("Registration error:", error);
    rej();
    return false;
  }
}

export async function SignIn(name, pass, lin, ade) {
  console.log("SignIn attempt:", name);
  
  try {
    const user = users.find(user => user.name === name && user.pass === pass);
    if (user) {
      localStorage.setItem(AUTH_KEY, name);
      console.log("SignIn successful, currentUser:", name);
      
      lin();
      return true;
    } else {
      console.log("SignIn failed: invalid credentials");
      ade();
      return false;
    }
  } catch (error) {
    console.log("SignIn error:", error);
    ade();
    return false;
  }
}

export async function SignOut(sou) {
  console.log("SignOut");
  
  localStorage.removeItem(AUTH_KEY);
  
  sou();
  return true;
}

export async function GetUserName(setA, setN) {
  const currentUser = localStorage.getItem(AUTH_KEY);
  console.log("GetUserName, currentUser:", currentUser);
  
  if (currentUser) {
    setA(currentUser);
  } else {
    setN();
  }
}