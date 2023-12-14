// You continue following signs for "Hot Springs" and eventually come across an observatory. The Elf within turns out to be a researcher studying cosmic expansion using the giant telescope here.
//
// He doesn't know anything about the missing machine parts; he's only visiting for this research project. However, he confirms that the hot springs are the next-closest area likely to have people; he'll even take you straight there once he's done with today's observation analysis.
//
// Maybe you can help him with the analysis to speed things up?
//
// The researcher has collected a bunch of data and compiled the data into a single giant image (your puzzle input). The image includes empty space (.) and galaxies (#). For example:
//
// ...#......
// .......#..
// #.........
// ..........
// ......#...
// .#........
// .........#
// ..........
// .......#..
// #...#.....
// The researcher is trying to figure out the sum of the lengths of the shortest path between every pair of galaxies. However, there's a catch: the universe expanded in the time it took the light from those galaxies to reach the observatory.
//
// Due to something involving gravitational effects, only some space expands. In fact, the result is that any rows or columns that contain no galaxies should all actually be twice as big.
//
// In the above example, three columns and two rows contain no galaxies:
//
// v  v  v
// ...#......
// .......#..
// #.........
// >..........<
// ......#...
// .#........
// .........#
// >..........<
// .......#..
// #...#.....
// ^  ^  ^
// These rows and columns need to be twice as big; the result of cosmic expansion therefore looks like this:
//
// ....#........
// .........#...
// #............
// .............
// .............
// ........#....
// .#...........
// ............#
// .............
// .............
// .........#...
// #....#.......
// Equipped with this expanded universe, the shortest path between every pair of galaxies can be found. It can help to assign every galaxy a unique number:
//
// ....1........
// .........2...
// 3............
// .............
// .............
// ........4....
// .5...........
// ............6
// .............
// .............
// .........7...
// 8....9.......
// In these 9 galaxies, there are 36 pairs. Only count each pair once; order within the pair doesn't matter. For each pair, find any shortest path between the two galaxies using only steps that move up, down, left, or right exactly one . or # at a time. (The shortest path between two galaxies is allowed to pass through another galaxy.)
//
// For example, here is one of the shortest paths between galaxies 5 and 9:
//
// ....1........
// .........2...
// 3............
// .............
// .............
// ........4....
// .5...........
// .##.........6
// ..##.........
// ...##........
// ....##...7...
// 8....9.......
// This path has length 9 because it takes a minimum of nine steps to get from galaxy 5 to galaxy 9 (the eight locations marked # plus the step onto galaxy 9 itself). Here are some other example shortest path lengths:
//
// Between galaxy 1 and galaxy 7: 15
// Between galaxy 3 and galaxy 6: 17
// Between galaxy 8 and galaxy 9: 5
// In this example, after expanding the universe, the sum of the shortest path between all 36 pairs of galaxies is 374.
//
// Expand the universe, then find the length of the shortest path between every pair of galaxies. What is the sum of these lengths?

use crate::custom_error::AocError;
use helpers::Point;

pub fn process(input: &str) -> miette::Result<u32, AocError> {
    // let input = include_str!("../test_input.txt");
    let mut universe: Vec<Vec<char>> = input
        .lines()
        .map(|line| line.chars().collect::<Vec<char>>())
        .collect();

    expand_universe(&mut universe);
    let galaxies = find_galaxies(&universe);

    let mut result = 0;
    for i in 0..galaxies.len() - 1 {
        for j in i + 1..galaxies.len() {
            let distance = find_path(galaxies[i], galaxies[j]);
            result += distance;
        }
    }

    Ok(result)
}

fn find_path(one: Point, two: Point) -> u32 {
    let result = (one.x - two.x).abs() + (one.y - two.y).abs();
    result as u32
}

fn find_galaxies(universe: &Vec<Vec<char>>) -> Vec<Point> {
    let mut galaxies = Vec::new();
    for (row, line) in universe.iter().enumerate() {
        for (col, c) in line.iter().enumerate() {
            if *c == '#' {
                galaxies.push(Point::new(row as i32, col as i32));
            }
        }
    }

    galaxies
}

fn expand_universe(universe: &mut Vec<Vec<char>>) {
    let mut rows_to_expand = universe
        .iter()
        .enumerate()
        .filter(|(_, row)| row.iter().all(|&c| c == '.'))
        .map(|(i, _)| i)
        .collect::<Vec<usize>>();

    while let Some(row) = rows_to_expand.pop() {
        universe.insert(row, universe[row].clone());
    }

    let mut cols_to_expand = Vec::new();
    for col in 0..universe[0].len() {
        if universe.iter().all(|row| row[col] == '.') {
            cols_to_expand.push(col);
        }
    }

    while let Some(col) = cols_to_expand.pop() {
        for row in 0..universe.len() {
            universe[row].insert(col, '.');
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");

        assert_eq!(374, process(input)?);
        Ok(())
    }
}
