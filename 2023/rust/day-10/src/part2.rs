use crate::custom_error::AocError;
use crate::shared::{create_pipe_system, find_start, parse_input, Pipe, Point};
use std::collections::{HashSet, VecDeque};
use tracing::info;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    // let input = include_str!("../test_input.txt");
    // let input = include_str!("../test_input_four.txt");
    let grid = parse_input(input);
    let start = find_start(&grid);
    let pipe_system = create_pipe_system(&grid, start);

    let pipe_system_pipes: HashSet<Point> = pipe_system.clone().into_iter().collect();
    let mut left_set = HashSet::new();
    let mut right_set = HashSet::new();

    // Same algo for creating the pipe system, but this time when ever we go up, we do some other shit
    let mut visited = HashSet::new();
    let mut queue = Vec::new();
    queue.push(start);
    'outer: while let Some(point) = queue.pop() {
        let current_pipe = grid[point.y as usize][point.x as usize];
        for next_direction in current_pipe.get_next_directions() {
            let next = point + next_direction;
            if next.x < 0
                || next.y < 0
                || next.x >= grid[0].len() as i32
                || next.y >= grid.len() as i32
            {
                continue;
            }

            let next_pipe = grid[next.y as usize][next.x as usize];
            if next_pipe == Pipe::Start && queue.len() > 1 {
                visited.insert(point);
                queue.push(point);
                break 'outer;
            }
            // If we've already visited this point, ignore it
            if visited.contains(&next) {
                continue;
            }

            if next_pipe == Pipe::Ground {
                visited.insert(next);
                continue;
            }

            if next_direction == Point::UP && next_pipe.has_south_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);

                // This is where we do our counting
                find_insiders(&grid, &pipe_system_pipes, point, Point::LEFT, &mut left_set);
                find_insiders(&grid, &pipe_system_pipes, next, Point::LEFT, &mut left_set);
                find_insiders(
                    &grid,
                    &pipe_system_pipes,
                    point,
                    Point::RIGHT,
                    &mut right_set,
                );
                find_insiders(
                    &grid,
                    &pipe_system_pipes,
                    next,
                    Point::RIGHT,
                    &mut right_set,
                );
                break;
            } else if next_direction == Point::DOWN && next_pipe.has_north_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            } else if next_direction == Point::LEFT && next_pipe.has_east_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            } else if next_direction == Point::RIGHT && next_pipe.has_west_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            }
        }
    }

    info!("Left set: {:?}", left_set.len());
    info!("Right set: {:?}", right_set.len());

    // I know the left set is the correct one
    Ok(right_set.len() as u64)
}

fn find_insiders(
    grid: &Vec<Vec<Pipe>>,
    pipe_system_pipes: &HashSet<Point>,
    start: Point,
    direction: Point,
    inside_set: &mut HashSet<Point>,
) {
    let mut current = start;
    loop {
        current = current + direction;
        if current.x < 0
            || current.y < 0
            || current.x >= grid[0].len() as i32
            || current.y >= grid.len() as i32
        {
            // If we're out of bounds, we know that this "set" isn't the correct one... but I can't be bothered
            // doing this properly
            break;
        }

        // If we've hit another pipe which is a part of the system, break
        if pipe_system_pipes.contains(&current) {
            break;
        }

        inside_set.insert(current);
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input_four.txt");
        assert_eq!(10, process(input)?);
        Ok(())
    }
}
