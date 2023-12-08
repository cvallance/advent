// You're still riding a camel across Desert Island when you spot a sandstorm quickly approaching. When you turn to warn the Elf, she disappears before your eyes! To be fair, she had just finished warning you about ghosts a few minutes ago.
//
// One of the camel's pouches is labeled "maps" - sure enough, it's full of documents (your puzzle input) about how to navigate the desert. At least, you're pretty sure that's what they are; one of the documents contains a list of left/right instructions, and the rest of the documents seem to describe some kind of network of labeled nodes.
//
// It seems like you're meant to use the left/right instructions to navigate the network. Perhaps if you have the camel follow the same instructions, you can escape the haunted wasteland!
//
// After examining the maps for a bit, two nodes stick out: AAA and ZZZ. You feel like AAA is where you are now, and you have to follow the left/right instructions until you reach ZZZ.
//
// This format defines each node of the network individually. For example:
//
// RL
//
// AAA = (BBB, CCC)
// BBB = (DDD, EEE)
// CCC = (ZZZ, GGG)
// DDD = (DDD, DDD)
// EEE = (EEE, EEE)
// GGG = (GGG, GGG)
// ZZZ = (ZZZ, ZZZ)
// Starting with AAA, you need to look up the next element based on the next left/right instruction in your input. In this example, start with AAA and go right (R) by choosing the right element of AAA, CCC. Then, L means to choose the left element of CCC, ZZZ. By following the left/right instructions, you reach ZZZ in 2 steps.
//
// Of course, you might not find ZZZ right away. If you run out of left/right instructions, repeat the whole sequence of instructions as necessary: RL really means RLRLRLRLRLRLRLRL... and so on. For example, here is a situation that takes 6 steps to reach ZZZ:
//
// LLR
//
// AAA = (BBB, BBB)
// BBB = (AAA, ZZZ)
// ZZZ = (ZZZ, ZZZ)
// Starting at AAA, follow the left/right instructions. How many steps are required to reach ZZZ?

use crate::custom_error::AocError;
use crate::shared::parse_input;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    // let input = include_str!("../test_input.txt");
    let (instructions, nodes) = parse_input(input);
    let mut location = "AAA";
    let mut result: u64 = 0;

    'outer: loop {
        for instruction in &instructions {
            if location == "ZZZ" {
                break 'outer;
            }

            result += 1;
            let node = nodes.get(location).unwrap();
            match instruction {
                'L' => location = &node.left,
                'R' => location = &node.right,
                _ => panic!("Unknown instruction: {}", instruction),
            }
        }
    }

    Ok(result)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(6, process(input)?);
        Ok(())
    }
}
