use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;
use std::{collections::VecDeque, str::from_utf8};

#[derive(Clone)]
pub struct IntcodeComputer {
    pub intcodes: Vec<i32>,
    instruction_pointer: usize,
    pub inputs: VecDeque<i32>,
    pub outputs: VecDeque<i32>,
    pub halted: bool,
}

#[derive(Debug)]
struct Instruction {
    opcode: u8,
    param_modes: [u8; 3],
}

impl Instruction {
    fn new(opvalue: i32) -> Instruction {
        let opcode = (opvalue % 100) as u8;
        let param_one_mode = if opvalue > 99 {
            (opvalue / 100 % 10) as u8
        } else {
            0
        };
        let param_two_mode = if opvalue > 999 {
            (opvalue / 1_000 % 10) as u8
        } else {
            0
        };
        let param_three_mode = if opvalue > 9999 {
            (opvalue / 10_000 % 10) as u8
        } else {
            0
        };
        Instruction {
            opcode,
            param_modes: [param_one_mode, param_two_mode, param_three_mode],
        }
    }
}

impl IntcodeComputer {
    pub fn from_file<P>(filename: P) -> IntcodeComputer
    where
        P: AsRef<Path>,
    {
        let file = File::open(filename).unwrap();
        let reader = io::BufReader::new(file);
        let mut values = Vec::new();
        for vars in reader.split(b',') {
            let bytes = vars.unwrap();
            if bytes.len() == 0 {
                continue;
            }
            let s = from_utf8(&bytes).unwrap();
            if let Ok(value) = s.trim().parse::<i32>() {
                values.push(value);
            }
        }
        IntcodeComputer {
            intcodes: values,
            instruction_pointer: 0,
            inputs: VecDeque::new(),
            outputs: VecDeque::new(),
            halted: false,
        }
    }

    pub fn from_vec(intcodes: Vec<i32>) -> IntcodeComputer {
        IntcodeComputer {
            intcodes,
            instruction_pointer: 0,
            inputs: VecDeque::new(),
            outputs: VecDeque::new(),
            halted: false,
        }
    }

    pub fn get_at_index(&self, index: usize) -> i32 {
        *self.intcodes.get(index).unwrap()
    }

    pub fn restore_and_process(&mut self, noun: i32, verb: i32) {
        self.intcodes[1] = noun;
        self.intcodes[2] = verb;

        self.process_intcodes();
    }

    pub fn process_intcodes(&mut self) {
        loop {
            let instruction = Instruction::new(self.intcodes[self.instruction_pointer]);
            match instruction.opcode {
                1 => {
                    self.op_code_one(instruction);
                }
                2 => {
                    self.op_code_two(instruction);
                }
                3 => match self.inputs.pop_front() {
                    Some(input) => {
                        self.op_code_three(input);
                    }
                    None => return,
                },
                4 => {
                    self.op_code_four(instruction);
                }
                5 => {
                    self.op_code_five(instruction);
                }
                6 => {
                    self.op_code_six(instruction);
                }
                7 => {
                    self.op_code_seven(instruction);
                }
                8 => {
                    self.op_code_eight(instruction);
                }
                99 => {
                    self.halted = true;
                    return;
                }
                _ => panic!(format!(
                    "Invalid opcode {} at position {}",
                    instruction.opcode, self.instruction_pointer
                )),
            }
        }
    }

    fn op_code_one(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        let param2 = self.get_param(2, &instruction);

        self.set_param(3, param1 + param2);
        self.instruction_pointer += 4;
    }

    fn op_code_two(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        let param2 = self.get_param(2, &instruction);
        self.set_param(3, param1 * param2);
        self.instruction_pointer += 4;
    }

    fn op_code_three(&mut self, input: i32) {
        self.set_param(1, input);
        self.instruction_pointer += 2;
    }

    // Opcode 4 outputs the value of its only parameter.
    // For example, the instruction 4,50 would output the value at address 50.
    fn op_code_four(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        self.outputs.push_back(param1);
        self.instruction_pointer += 2;
    }

    // Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter.
    // Otherwise, it does nothing.
    fn op_code_five(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        if param1 != 0 {
            let param2 = self.get_param(2, &instruction) as usize;
            self.instruction_pointer = param2;
        } else {
            self.instruction_pointer += 3;
        }
    }

    // Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter.
    // Otherwise, it does nothing.
    fn op_code_six(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        if param1 == 0 {
            let param2 = self.get_param(2, &instruction) as usize;
            self.instruction_pointer = param2;
        } else {
            self.instruction_pointer += 3;
        }
    }

    // Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter.
    // Otherwise, it stores 0.
    fn op_code_seven(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        let param2 = self.get_param(2, &instruction);
        self.set_param(3, if param1 < param2 { 1 } else { 0 });

        self.instruction_pointer += 4;
    }

    // Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.
    // Otherwise, it stores 0.
    fn op_code_eight(&mut self, instruction: Instruction) {
        let param1 = self.get_param(1, &instruction);
        let param2 = self.get_param(2, &instruction);
        self.set_param(3, if param1 == param2 { 1 } else { 0 });

        self.instruction_pointer += 4;
    }

    fn set_param(&mut self, param_number: usize, new_value: i32) {
        let write_location = self.intcodes[self.instruction_pointer + param_number] as usize;
        self.intcodes[write_location] = new_value;
    }

    fn get_param(&self, param_number: usize, instruction: &Instruction) -> i32 {
        let mode = instruction.param_modes[param_number - 1];
        match mode {
            0 => {
                let location: usize = self.instruction_pointer + param_number;
                let location = self.intcodes[location] as usize;
                return self.intcodes[location];
            }
            1 => {
                let location: usize = self.instruction_pointer + param_number;
                return self.intcodes[location];
            }
            _ => panic!("Unknown param mode: {}", mode),
        }
    }
}
